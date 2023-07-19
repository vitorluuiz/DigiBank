using Bogus;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace digibank_back.Repositories
{
    public class InvestimentoOptionsRepository : IInvestimentoOptionsRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Atualizar(int idInvestimentoOption, InvestimentoOption optionAtualizada)
        {
            InvestimentoOption optionDesatualizada = ListarPorId(idInvestimentoOption);

            optionDesatualizada.Nome = optionAtualizada.Nome;
            optionDesatualizada.IdTipoInvestimento = optionAtualizada.IdTipoInvestimento;
            optionDesatualizada.Sigla = optionAtualizada.Sigla;
            optionDesatualizada.ValorAcao = optionAtualizada.ValorAcao;
            optionDesatualizada.Descricao = optionAtualizada.Descricao;
            optionDesatualizada.PercentualDividendos = optionAtualizada.PercentualDividendos;

            ctx.Update(optionDesatualizada);
            ctx.SaveChanges();
        }

        public InvestimentoOption CreateFicOption()
        {
            InvestimentoOption antOption = ctx.InvestimentoOptions.FirstOrDefault(O => O.Nome == "Teste");

            if (antOption != null)
            {
                ctx.HistoricoInvestimentoOptions.RemoveRange(ctx.HistoricoInvestimentoOptions.Where(H => H.IdInvestimentoOption == antOption.IdInvestimentoOption));
                ctx.InvestimentoOptions.Remove(antOption);
                ctx.SaveChanges();
            }

            var lorem = new Bogus.DataSets.Lorem(locale: "pt_BR");
            Console.WriteLine(lorem.Sentence(5));

            InvestimentoOption option = new Faker<InvestimentoOption>("pt_BR")
                .RuleFor(o => o.Nome, p => $"{p.Company.CompanyName()} {p.Company.CompanySuffix()}")
                .RuleFor(o => o.Descricao, p => p.Company.CatchPhrase())
                .RuleFor(o => o.Abertura, p => p.Date.Past(5, DateTime.Now))
                .RuleFor(o => o.Fundacao, p => p.Date.Past(5, DateTime.Now))
                .RuleFor(o => o.Fundador, p => $"{p.Name.Prefix()} {p.Name.FullName()}")
                .RuleFor(o => o.ValorAcao, p => p.Random.Number(10, 400))
                .RuleFor(o => o.Colaboradores, p => p.Random.Int(30, 150000))
                .RuleFor(o => o.Sede, p => $"{p.Address.City()}, {p.Address.Country()}")
                .RuleFor(o => o.IdAreaInvestimento, p => p.Random.Short(1, ctx.AreaInvestimentos.OrderBy(a => a.IdAreaInvestimento).Last().IdAreaInvestimento))
                .RuleFor(o => o.IdTipoInvestimento, p => p.Random.Byte(1, ctx.TipoInvestimentos.OrderBy(a => a.IdTipoInvestimento).Last().IdTipoInvestimento))
                .RuleFor(o => o.Logo, "NaoTemLogoAinda.png")
                .RuleFor(o => o.MainImg, p => p.Image.PicsumUrl(640, 480, false, false, null))
                .RuleFor(o => o.MainColorHex, p => (p.Random.Int(111111, 999999)).ToString())
                .RuleFor(o => o.PercentualDividendos, p => p.Random.Decimal(0, 10))
                .RuleFor(o => o.QntCotasTotais, p => p.Random.Short(10000, short.MaxValue))
                .Generate();

            option.Sigla = option.Nome.Substring(0, 6).ToUpper();
            option.Tick = DateTime.Now.AddDays(-700);
            ctx.InvestimentoOptions.Add(option);
            ctx.SaveChanges();

            return option;
        }

        public List<EmblemaInvestOption> ListarEmblemas(int idInvestimentoOption, int days)
        {
            InvestimentoOption option = ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == idInvestimentoOption);

            if (option == null)
            {
                return null;
            }

            EmblemaInvestOption emblemaCreator = new EmblemaInvestOption();
            List<EmblemaInvestOption> emblemasPadrao = emblemaCreator.GetEmblemas();
            List<EmblemaInvestOption> emblemasOption = new List<EmblemaInvestOption>();
            List<InvestimentoOption> options = ctx.InvestimentoOptions
                .Where(O => O.IdTipoInvestimento != 1 &&
                O.IdTipoInvestimento != 2)
                .ToList();

            double index = 0;
            double percentil = 0;

            //Emblemas por dividendo
            options = options.OrderBy(O => O.PercentualDividendos).ToList();
            index = options.FindIndex(O => O.PercentualDividendos > option.PercentualDividendos);
            if (index == -1)
            {
                percentil = 0;
            }
            else
            {
                percentil = 1 - (index / (options.Count - 1));
            }
            emblemasOption.Add(emblemasPadrao.Where(E => E.Tipo == 1 && E.Corte >= percentil)
                .OrderBy(O => O.Valor)
                .FirstOrDefault());

            //Emblema por ValorAcao
            options = options.OrderByDescending(O => O.ValorAcao).ToList();
            index = options.FindIndex(O => O.ValorAcao >= option.ValorAcao);
            if (index == -1)
            {
                percentil = 0;
            }
            else
            {
                percentil = 1 - (index / (options.Count - 1));
            }
            emblemasOption.Add(emblemasPadrao.Where(E => E.Tipo == 2 && E.Corte >= percentil)
                .OrderBy(O => O.Valor)
                .FirstOrDefault());

            //Emblema por MarketCap
            options.OrderBy(O => O.ValorAcao * O.QntCotasTotais).ToList();
            index = options.FindIndex(O => O.ValorAcao * O.QntCotasTotais > option.ValorAcao * option.QntCotasTotais);
            if (index == -1)
            {
                percentil = 0;
            }
            else
            {
                percentil = 1 - (index / (options.Count - 1));
            }
            emblemasOption.Add(emblemasPadrao.Where(E => E.Tipo == 3 && E.Corte >= percentil)
                .OrderBy(O => O.Valor)
                .FirstOrDefault());

            return emblemasOption;
        }

        public List<double> ListarIndices(int idInvestimentoOption, int days)
        {
            InvestimentoOption option = ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == idInvestimentoOption);

            if (option == null)
            {
                return null;
            }

            HistoryInvestRepository historyInvestRepository = new();
            List<InvestimentoOption> options = ctx.InvestimentoOptions
                .Where(O => O.IdTipoInvestimento != 1 &&
                O.IdTipoInvestimento != 2)
                .ToList();

            List<StatsHistoryOption> statsOptions = new();
            foreach (InvestimentoOption item in options)
            {
                StatsHistoryOption statsItem = ListarStatsHistoryOption(item.IdInvestimentoOption, days);
                statsOptions.Add(statsItem);
            }

            List<double> indices = new();
            double index = 0;
            double percentil = 0;
            double min = 4;
            double max = 10;

            //ValorAcao
            options = options.OrderBy(O => O.ValorAcao).ToList();
            index = options.FindIndex(O => O.ValorAcao >= option.ValorAcao);
            percentil = 1 - (index / (options.Count - 1));
            indices.Add(max - percentil * (max - min));

            //Dividendos
            options = options.OrderBy(O => O.PercentualDividendos).ToList();
            index = options.FindIndex(O => O.PercentualDividendos >= option.PercentualDividendos);
            percentil = 1 - (index / (options.Count - 1));
            indices.Add(max - percentil * (max - min));

            //Valorização por algum motivo não ordena corretamente
            StatsHistoryOption statsOption = statsOptions.FirstOrDefault(O => O.IdInvestimentoOption == idInvestimentoOption);
            statsOptions.OrderByDescending(O => O.VariacaoPeriodoPercentual).ToList();
            index = statsOptions.FindIndex(O => O.VariacaoPeriodoPercentual >= statsOption.VariacaoPeriodoPercentual);
            percentil = 1 - (index / (options.Count - 1));
            indices.Add(max - percentil * (max - min));

            //Confiabilidade
            statsOptions = statsOptions.OrderByDescending(o => Math.Abs(1 - o.CoeficienteVariativo)).ToList();
            index = statsOptions.FindIndex(O => O.CoeficienteVariativo >= statsOption.CoeficienteVariativo);
            percentil = 1 - (index / (options.Count - 1));
            indices.Add(max - percentil * (max - min));

            return indices;
        }

        public InvestimentoOption ListarPorId(int idInvestimentoOption)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdInvestimentoOption == idInvestimentoOption);
        }

        public StatsHistoryOption ListarStatsHistoryOption(int idOption, int days)
        {
            InvestimentoOption option = ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == idOption);
            if (option == null) return null;

            HistoryInvestRepository historyInvestRepository = new();
            List<HistoricoInvestimentoOption> history = historyInvestRepository.GetHistoryFromOption(idOption, days);

            decimal max = history.Max(O => O.Valor);
            decimal min = history.Min(O => O.Valor);
            decimal media = history.Average(O => O.Valor);

            return new StatsHistoryOption()
            {
                IdInvestimentoOption = idOption,
                IdTipoInvestimento = option.IdTipoInvestimento,
                Max = Math.Round(max, 2),
                Min = Math.Round(min, 2),
                Media = Math.Round(media, 2),
                VariacaoMinMax = Math.Round(max - min, 2),
                VariacaoMinMaxPorcentual = Math.Round((max - min) / option.ValorAcao * 100, 2),
                VariacaoPeriodo = Math.Round(history[0].Valor - history[^1].Valor, 2),
                VariacaoPeriodoPercentual = Math.Round((history[0].Valor - history[^1].Valor) / option.ValorAcao * 100, 2),
                CoeficienteVariativo = Math.Round((max - media) / (media - min), 2)
            };
        }

        public List<InvestimentoOption> ListarTodos(int pagina, int qntItens)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }

        public List<InvestimentoOption> ListarTodosPorId(int[] ids)
        {
            return ctx.InvestimentoOptions
                .Where(I => ids.Contains(I.IdInvestimentoOption))
                .ToList();
        }
    }
}
