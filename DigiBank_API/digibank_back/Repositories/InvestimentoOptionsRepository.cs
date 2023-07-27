using Bogus;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using digibank_back.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace digibank_back.Repositories
{
    public class InvestimentoOptionsRepository : IInvestimentoOptionsRepository
    {
        private readonly digiBankContext _ctx;
        private readonly HistoryInvestRepository _historyInvestRepository;
        private readonly IMemoryCache _memoryCache;

        public InvestimentoOptionsRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _historyInvestRepository = new HistoryInvestRepository(ctx, memoryCache);
            _memoryCache = memoryCache;
        }

        public void Update(short id, InvestimentoOption updatedO)
        {
            var option = ListarPorId(id);

            option.Nome = updatedO.Nome;
            option.IdTipo = updatedO.IdTipoInvestimento;
            option.Sigla = updatedO.Sigla;
            option.Valor = updatedO.ValorAcao;
            option.Descricao = updatedO.Descricao;
            option.Dividendos = updatedO.PercentualDividendos;

            _ctx.Update(option);
            _ctx.SaveChanges();
        }

        public InvestimentoOption CreateFicOption()
        {
            var option = new Faker<InvestimentoOption>()
                .RuleFor(o => o.Nome, p => $"{p.Company.CompanyName()} {p.Company.CompanySuffix()}")
                .RuleFor(o => o.Descricao, p => p.Company.CatchPhrase())
                .RuleFor(o => o.Abertura, p => p.Date.Past(5, DateTime.Now))
                .RuleFor(o => o.Fundacao, p => p.Date.Past(5, DateTime.Now))
                .RuleFor(o => o.Fundador, p => $"{p.Name.Prefix()} {p.Name.FullName()}")
                .RuleFor(o => o.ValorAcao, p => p.Random.Number(10, 400))
                .RuleFor(o => o.Colaboradores, p => p.Random.Int(30, 150000))
                .RuleFor(o => o.Sede, p => $"{p.Address.City()}, {p.Address.Country()}")
                .RuleFor(o => o.IdAreaInvestimento, p => p.Random.Short(1, _ctx.AreaInvestimentos.OrderBy(a => a.IdAreaInvestimento).Last().IdAreaInvestimento))
                .RuleFor(o => o.IdTipoInvestimento, p => p.Random.Byte(1, _ctx.TipoInvestimentos.OrderBy(a => a.IdTipoInvestimento).Last().IdTipoInvestimento))
                .RuleFor(o => o.MainImg, p => p.Image.PicsumUrl(640, 480, false, false, null))
                .RuleFor(o => o.MainColorHex, p => (p.Random.Int(111111, 999999)).ToString())
                .RuleFor(o => o.PercentualDividendos, p => p.Random.Decimal(0, 10))
                .RuleFor(o => o.QntCotasTotais, p => p.Random.Short(10000, short.MaxValue))
                .Generate();

            Random random = new();
            int logoSkips = random.Next(1, MockData.Logo.Count());
            option.Logo = $"https://img.logoipsum.com/{MockData.Logo.Get(logoSkips)}.svg";
            if (option.Nome.Length >= 6) option.Sigla = option.Nome[..6].ToUpper();
            //option.Fundacao = new Faker<DateTime>().RuleFor(d => d.Date, p => p.Date.Past(5, DateTime.Now)).Generate();
            //option.Abertura = new Faker<DateTime>().RuleFor(d => d.Date, p => p.Date.Past(Convert.ToInt32(DateTime.Now - option.Fundacao), DateTime.Now));
            option.Tick = DateTime.Now.AddDays(-365);
            _ctx.InvestimentoOptions.Add(option);
            _ctx.SaveChanges();

            return option;
        }

        public List<EmblemaInvestOption> ListarEmblemas(int idOption, int days)
        {
            InvestimentoOption option = _ctx.InvestimentoOptions
                .FirstOrDefault(o => o.IdInvestimentoOption == idOption &&
                o.IdTipoInvestimento != 1 &&
                o.IdTipoInvestimento != 2);

            if (option == null)
            {
                new List<EmblemaInvestOption>
                {
                    null, null, null
                };
            }

            var cache = new MemoryCacheProvider.Options.OrderBy(_memoryCache, _ctx);
            var emptyStats = new StatsHistoryOption
            {
                IdInvestimentoOption = idOption,
                Dividendos = option.PercentualDividendos,
                MarketCap = option.ValorAcao * option.QntCotasTotais,
            };
            var statsOption = new StatsInvestProvider(_ctx, _memoryCache).HistoryOptionStats(emptyStats, days);

            var emblemaCreator = new EmblemaInvestOption();
            List<EmblemaInvestOption> emblemasPadrao = emblemaCreator.GetEmblemas();
            List<EmblemaInvestOption> emblemasOption = new();

            //Emblemas por dividendo
            var percentilDividendos = StatsInvestProvider.CalculatePercentile(cache.Dividendos(), o => o.Dividendos, statsOption.Dividendos);
            emblemasOption.Add(emblemasPadrao.Where(E => E.Tipo == 1 && E.Corte >= percentilDividendos)
                .OrderBy(o => o.Valor)
                .FirstOrDefault());

            //Emblema por ValorAcao
            var percentilValorAcao = StatsInvestProvider.CalculatePercentile(cache.ValorAcao(), o => o.Valor, statsOption.Valor);
            emblemasOption.Add(emblemasPadrao.Where(E => E.Tipo == 2 && E.Corte <= percentilValorAcao)
                .OrderBy(O => O.Valor)
                .FirstOrDefault());

            //Emblema por MarketCap
            var percentilMarketCap = StatsInvestProvider.CalculatePercentile(cache.MarketCap(), o => o.MarketCap, statsOption.Valor);
            emblemasOption.Add(emblemasPadrao.Where(e => e.Tipo == 3 && e.Corte >= percentilMarketCap)
                .OrderBy(o => o.Valor)
                .FirstOrDefault());

            return emblemasOption;
        }

        public List<double> Indices(int idOption, int days)
        {
            InvestimentoOption option = _ctx.InvestimentoOptions
                 .FirstOrDefault(o => o.IdInvestimentoOption == idOption &&
                 o.IdTipoInvestimento != 1 &&
                 o.IdTipoInvestimento != 2);

            if (option == null || option.IdTipoInvestimento is 1 or 2)
            {
                return new List<double>
                {
                    0, 0, 0, 0
                };
            }

            List<double> indices = new();
            const double min = 4;
            const double max = 10;

            var cache = new MemoryCacheProvider.Options.OrderBy(_memoryCache, _ctx);
            var emptyStats = new StatsHistoryOption
            {
                IdInvestimentoOption = idOption,
                Dividendos = option.PercentualDividendos,
                MarketCap = option.ValorAcao * option.QntCotasTotais
            };
            var statsOption = new StatsInvestProvider(_ctx, _memoryCache).HistoryOptionStats(emptyStats, days);

            //Market cap
            var marketCapPercentil =
                StatsInvestProvider.CalculatePercentile(cache.MarketCap(), o => o.MarketCap, statsOption.MarketCap);
            indices.Add(max - marketCapPercentil * (max - min));

            //Dividendos
            var dividendosPercentil =
                StatsInvestProvider.CalculatePercentile(cache.Dividendos(), o => o.Dividendos, statsOption.Dividendos);
            indices.Add(max - dividendosPercentil * (max - min));

            //Valorização
            var valorizacaoPercentil =
                StatsInvestProvider.CalculatePercentile(cache.Valorizacao(), o => o.VariacaoPeriodoPercentual, statsOption.VariacaoPeriodoPercentual);
            indices.Add(max - valorizacaoPercentil * (max - min));

            //Confiabilidade
            var confiabilidadePercentil =
                StatsInvestProvider.CalculatePercentile(cache.Confiabilidade(), o => o.CoeficienteVariativo, statsOption.CoeficienteVariativo);
            indices.Add(max - confiabilidadePercentil * (max - min));

            return indices;
        }

        public InvestimentoOptionGenerico ListarPorId(int idInvestimentoOption)
        {
            HistoryInvestRepository historyInvestRepository = new(_ctx, _memoryCache);
            historyInvestRepository.UpdateOptionHistory(idInvestimentoOption);
            InvestimentoOption option = _ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .Include(f => f.IdAreaInvestimentoNavigation)
                .FirstOrDefault(f => f.IdInvestimentoOption == idInvestimentoOption);

            if (option != null)
            {
                InvestimentoOptionGenerico optionGenerico = new(option);
                optionGenerico.VariacaoPercentual = 1; // Corrigir

                return optionGenerico;
            }

            return null;
        }

        public List<InvestimentoOptionMinimo> ListarTodos(int pagina, int qntItens)
        {
            return _ctx.InvestimentoOptions
                .Include(I => I.IdTipoInvestimentoNavigation)
                .Include(I => I.IdAreaInvestimentoNavigation)
                .Select(f => new InvestimentoOptionMinimo(f))
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }
        public List<InvestimentoOptionMinimo> ListarCompradosAnteriormente(int pagina, int qntItens, byte idTipoInvestimentoOption, int idUsuario)
        {
            InvestimentoRepository investimentoRepository = new(_ctx, _memoryCache);
            List<InvestimentoOptionMinimo> compradosAnteriormente = new();
            HashSet<int> idsAdicionados = new HashSet<int>();
            int paginacao = pagina;
            List<Investimento> investimentos = new();

            do
            {
                investimentos = investimentoRepository.ListarDeUsuario(idUsuario, paginacao, qntItens);

                foreach (Investimento item in investimentos)
                {
                    InvestimentoOption option = item.IdInvestimentoOptionNavigation;

                    if (option.IdTipoInvestimento == idTipoInvestimentoOption && idsAdicionados.Count < qntItens)
                    {
                        compradosAnteriormente.Add(new InvestimentoOptionMinimo(option));

                        idsAdicionados.Add(option.IdInvestimentoOption);
                    }
                }
                paginacao++;
            } while (idsAdicionados.Count != qntItens && investimentos.Count != 0);

            return compradosAnteriormente;
        }
        public List<InvestimentoTitle> BuscarInvestimentos(byte idTipoInvestimentoOption, int qntItens)
        {
            return _ctx.InvestimentoOptions
                .Include(o => o.IdTipoInvestimentoNavigation)
                .Where(o => o.IdTipoInvestimentoNavigation.IdTipoInvestimento == idTipoInvestimentoOption)
                .Select(o => new InvestimentoTitle
                {
                    IdInvestimentoOption = o.IdInvestimentoOption,
                    Nome = o.Nome,
                    Valor = o.ValorAcao,
                    Logo = o.Logo
                })
                .Take(qntItens)
                .ToList();
        }
        public List<InvestimentoOptionMinimo> ListarPorTipoInvestimento(byte idTipoInvestimentoOption, int pagina, int qntItens)
        {
            return _ctx.InvestimentoOptions
                .Where(f => f.IdTipoInvestimentoNavigation.IdTipoInvestimento == idTipoInvestimentoOption)
                .Include(I => I.IdTipoInvestimentoNavigation)
                .Include(I => I.IdAreaInvestimentoNavigation)
                .Select(f => new InvestimentoOptionMinimo(f))
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }
        public List<InvestimentoOptionMinimo> ListarTodosPorId(int[] ids)
        {
            return _ctx.InvestimentoOptions
                .Where(I => ids.Contains(I.IdInvestimentoOption))
                .Include(I => I.IdTipoInvestimentoNavigation)
                .Include(I => I.IdAreaInvestimentoNavigation)
                .Select(I => new InvestimentoOptionMinimo(I))
                .ToList();
        }
    }
}