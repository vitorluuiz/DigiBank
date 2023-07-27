using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Utils
{
    public class MemoryCacheProvider
    {
        public class Options
        {
            private readonly digiBankContext _ctx;
            private readonly IMemoryCache _memoryCache;
            public Options(digiBankContext ctx, IMemoryCache memoryCache)
            {
                _ctx = ctx;
                _memoryCache = memoryCache;
            }

            public List<StatsHistoryOption> GetFromDb()
            {
                var list = _ctx.InvestimentoOptions
                    .Where(o => o.IdTipoInvestimento != 1 &&
                    o.IdTipoInvestimento != 2)
                    .Select(o => new StatsHistoryOption
                    {
                        IdInvestimentoOption = o.IdInvestimentoOption,
                        Dividendos = o.PercentualDividendos,
                        Valor = o.ValorAcao,
                        MarketCap = o.ValorAcao * o.QntCotasTotais
                    })
                    .ToList();

                return list;
            }

            public List<StatsHistoryOption> GetFullStats(int days)
            {
                var dbList = GetFromDb();
                var list = new List<StatsHistoryOption>();

                foreach (var item in dbList)
                {
                    list.Add(new StatsInvestProvider(_ctx, _memoryCache).HistoryOptionStats(item, days));
                }

                return list;
            }

            public List<StatsHistoryOption> CreateDefaultTable(Func<StatsHistoryOption, decimal> selector, int days)
            {
                var orderedList = GetFullStats(days).OrderBy(selector).ToList();
                var percentilTable = new List<StatsHistoryOption>();

                int count = orderedList.Count;
                double intervalSize = (double)count / 100;

                for (int i = 0; i < 100; i++)
                {
                    int index = (int)Math.Round(i * intervalSize);
                    index = Math.Min(index, orderedList.Count - 1);
                    percentilTable.Add(orderedList[index]);
                }

                return percentilTable;
            }

            public List<StatsHistoryOption> CreateCoeficienteTable(int expected, int days)
            {
                var orderedList = GetFullStats(days).OrderBy(o => Math.Abs(expected - o.CoeficienteVariativo)).ToList();
                var percentilTable = new List<StatsHistoryOption>();

                int count = orderedList.Count;
                double intervalSize = (double)count / 100;

                for (int i = 0; i < 100; i++)
                {
                    int index = (int)Math.Round(i * intervalSize);
                    index = Math.Min(index, orderedList.Count - 1);
                    percentilTable.Add(orderedList[index]);
                }

                return percentilTable;
            }

            public class OrderBy
            {
                private const string DIVIDENDOS_KEY = "Dividendos";
                private const string MARKETCAP_KEY = "MarketCap";
                private const string VALORIZACAO_KEY = "Valorizacao";
                private const string CONFIABILIDADE_KEY = "Confiabilidade";
                private const string VALOR_ACAO_KEY = "ValorAcao";

                private readonly IMemoryCache _memoryCache;
                private readonly digiBankContext _ctx;
                public OrderBy(IMemoryCache memoryCache, digiBankContext ctx)
                {
                    _memoryCache = memoryCache;
                    _ctx = ctx;
                }

                public List<StatsHistoryOption> Dividendos()
                {
                    if (_memoryCache.TryGetValue(DIVIDENDOS_KEY, out List<StatsHistoryOption> cacheList))
                    {
                        return cacheList;
                    }

                    var dbList = new Options(new digiBankContext(), _memoryCache).CreateDefaultTable(o => o.Dividendos, 30);

                    _memoryCache.Set(DIVIDENDOS_KEY, dbList);

                    return dbList;
                }

                public List<StatsHistoryOption> MarketCap()
                {
                    if (_memoryCache.TryGetValue(MARKETCAP_KEY, out List<StatsHistoryOption> cacheList))
                    {
                        return cacheList;
                    }

                    var optionsHandler = new Options(_ctx, _memoryCache);
                    var dbList = optionsHandler.CreateDefaultTable(o => o.MarketCap, 30);

                    _memoryCache.Set(MARKETCAP_KEY, dbList);

                    return dbList;
                }

                public List<StatsHistoryOption> Valorizacao()
                {
                    if (_memoryCache.TryGetValue(VALORIZACAO_KEY, out List<StatsHistoryOption> cacheList))
                    {
                        return cacheList;
                    }

                    var dbList = new Options(new digiBankContext(), _memoryCache).CreateDefaultTable(o => o.VariacaoPeriodoPercentual, 30);

                    _memoryCache.Set(VALORIZACAO_KEY, dbList);

                    return dbList;
                }

                public List<StatsHistoryOption> Confiabilidade()
                {
                    if (_memoryCache.TryGetValue(CONFIABILIDADE_KEY, out List<StatsHistoryOption> cacheList))
                    {
                        return cacheList;
                    }

                    var dbList = new Options(new digiBankContext(), _memoryCache).CreateCoeficienteTable(1, 30);

                    _memoryCache.Set(CONFIABILIDADE_KEY, dbList);

                    return dbList;
                }

                public List<StatsHistoryOption> ValorAcao()
                {
                    if (_memoryCache.TryGetValue(VALOR_ACAO_KEY, out List<StatsHistoryOption> cacheList))
                    {
                        return cacheList;
                    }

                    var dbList = new Options(new digiBankContext(), _memoryCache).CreateDefaultTable(o => o.Valor, 30);

                    _memoryCache.Set(VALOR_ACAO_KEY, dbList);

                    return dbList;
                }
            }
        }
    }
}
