﻿using Bogus;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using digibank_back.Utils;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;

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
            option.IdTipo = updatedO.IdAreaInvestimentoNavigation.IdTipoInvestimento;
            option.Sigla = updatedO.Sigla;
            option.Valor = updatedO.ValorAcao;
            option.Descricao = updatedO.Descricao;
            option.Dividendos = updatedO.PercentualDividendos;

            _ctx.Update(option);
            _ctx.SaveChanges();
        }

        public InvestimentoOption CreateFicOption()
        {
            var random = new Random();
            InvestimentoOption newOption = MockData.Option.MockAll(random.Next(1, 100));

            _ctx.InvestimentoOptions.Add(newOption);
            _ctx.SaveChanges();

            return newOption;
        }

        public List<EmblemaInvestOption> ListarEmblemas(int idOption, int days)
        {
            InvestimentoOption option = _ctx.InvestimentoOptions
                .FirstOrDefault(o => o.IdInvestimentoOption == idOption &&
                o.IdAreaInvestimentoNavigation.IdTipoInvestimento != 1 &&
                o.IdAreaInvestimentoNavigation.IdTipoInvestimento != 2);

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
            var percentilMarketCap = StatsInvestProvider.CalculatePercentile(cache.MarketCap(), o => o.MarketCap, statsOption.Valor * option.QntCotasTotais);
            emblemasOption.Add(emblemasPadrao.Where(e => e.Tipo == 3 && e.Corte >= percentilMarketCap)
                .OrderBy(o => o.Valor)
                .FirstOrDefault());

            return emblemasOption;
        }

        public List<double> Indices(int idOption, int days)
        {
            InvestimentoOption option = _ctx.InvestimentoOptions
                .Include(o => o.IdAreaInvestimentoNavigation)
                 .FirstOrDefault(o => o.IdInvestimentoOption == idOption &&
                 o.IdAreaInvestimentoNavigation.IdTipoInvestimento != 1 &&
                 o.IdAreaInvestimentoNavigation.IdTipoInvestimento != 2);

            if (option == null || option.IdAreaInvestimentoNavigation.IdTipoInvestimento is 1 or 2)
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
                .Include(f => f.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation)
                .FirstOrDefault(f => f.IdInvestimentoOption == idInvestimentoOption);

            if (option != null)
            {
                InvestimentoOptionGenerico optionGenerico = new(option);
                optionGenerico.VariacaoPercentual = 1; // Corrigir, Talvez tenha que ser feita uma requisição individual para cada option listada

                return optionGenerico;
            }

            return null;
        }

        public List<InvestimentoOptionMinimo> ListarCompradosAnteriormente(int pagina, int qntItens, byte idTipoInvestimentoOption, int idUsuario)
        {
            InvestimentoRepository investimentoRepository = new(_ctx, _memoryCache);
            List<InvestimentoOptionMinimo> compradosAnteriormente = new();
            HashSet<int> idsAdicionados = new HashSet<int>();
            int paginacao = pagina;
            List<InvestimentoGenerico> investimentos = new();

            do
            {
                investimentos = investimentoRepository.GetCarteira(idUsuario, idTipoInvestimentoOption, paginacao, qntItens);

                foreach (InvestimentoGenerico item in investimentos)
                {
                    InvestimentoOptionGenerico option = item.IdInvestimentoOptionNavigation;

                    if (option.IdTipo == idTipoInvestimentoOption && idsAdicionados.Count < qntItens)
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
                .Include(o => o.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation)
                .Where(o => o.IdAreaInvestimentoNavigation.IdTipoInvestimento == idTipoInvestimentoOption)
                .Take(qntItens)
                .Select(o => new InvestimentoTitle(o))
                .ToList();
        }

        public List<InvestimentoOptionMinimo> ListarTodosPorId(int[] ids, byte idTipoInvestimentoOption)
        {
            return _ctx.InvestimentoOptions
                .Where(I => ids.Contains(I.IdInvestimentoOption) && I.IdAreaInvestimentoNavigation.IdTipoInvestimento == idTipoInvestimentoOption)
                .Include(I => I.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation)
                .Select(I => new InvestimentoOptionMinimo(I))
                .ToList();
        }

        public List<InvestimentoOptionMinimo> AllWhere(Expression<Func<InvestimentoOption, bool>> predicado, int pagina, int qntItens)
        {
            return _ctx.InvestimentoOptions
                .Where(predicado)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .Include(I => I.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation)
                .Select(o => new InvestimentoOptionMinimo
                {
                    IdInvestimentoOption = o.IdInvestimentoOption,
                    TipoInvestimento = o.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation.TipoInvestimento1,
                    AreaInvestimento = o.IdAreaInvestimentoNavigation.Area,
                    Nome = o.Nome,
                    Sigla = o.Sigla,
                    Logo = o.Logo,
                    MainImg = o.MainImg,
                    MainColorHex = o.MainColorHex,
                    Valor = Math.Round(o.ValorAcao, 2),
                    Dividendos = o.PercentualDividendos
                })
                .ToList();
        }

        public int CountWhere(Expression<Func<InvestimentoOption, bool>> predicado)
        {
            return _ctx.InvestimentoOptions.Count(predicado);
        }
    }
}