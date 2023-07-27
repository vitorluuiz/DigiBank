using digibank_back.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using digibank_back.DTOs;
using digibank_back.Repositories;
using digibank_back.Contexts;
using Microsoft.Extensions.Caching.Memory;

namespace digibank_back.Utils
{
    public class StatsInvestProvider
    {
        private readonly digiBankContext _ctx;
        private readonly IMemoryCache _memoryCache;
        public StatsInvestProvider(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _memoryCache = memoryCache;
        }

        public StatsHistoryOption HistoryOptionStats(StatsHistoryOption option, int days)
        {
            List<HistoricoInvestimentoOption> history = new HistoryInvestRepository(_ctx, _memoryCache).GetHistoryFromOption(option.IdInvestimentoOption, days);

            decimal max = history.Max(O => O.Valor);
            decimal min = history.Min(O => O.Valor);
            decimal media = history.Average(O => O.Valor);
            decimal valorAnterior = history[0].Valor;
            decimal valorAtual = history[^1].Valor;
            decimal variacaoMinMax = max - min;
            decimal variacaoMinMaxPercentual = 0;
            if (variacaoMinMax != 0 && valorAtual != 0)
            {
                variacaoMinMaxPercentual = variacaoMinMax / valorAtual * 100;
            }

            decimal variacaoPeriodo = valorAnterior - valorAtual;
            decimal variacaoPeriodoPercentual = 0;
            if (variacaoPeriodo != 0 && valorAtual != 0)
            {
                variacaoPeriodoPercentual = variacaoPeriodo / valorAtual * 100;
            }
            else
            {
                variacaoPeriodoPercentual = variacaoPeriodo;
            }

            decimal coeficienteVariativo = 1;
            if (max - media != 0 && media - min != 0)
            {
                coeficienteVariativo = (max - media) / (media - min);
            }

            return new StatsHistoryOption()
            {
                IdInvestimentoOption = option.IdInvestimentoOption,
                Dividendos = option.Dividendos,
                MarketCap = option.MarketCap,
                Valor = option.Valor,
                Max = Math.Round(max, 2),
                Min = Math.Round(min, 2),
                Media = Math.Round(media, 2),
                MinMax = Math.Round(variacaoMinMax, 2),
                MinMaxPercentual = Math.Round(variacaoMinMaxPercentual, 2),
                VariacaoPeriodo = Math.Round(variacaoPeriodo, 2),
                VariacaoPeriodoPercentual = Math.Round(variacaoPeriodoPercentual, 2),
                CoeficienteVariativo = Math.Round(coeficienteVariativo, 2),
            };
        }

        public static double CalculatePercentile(List<StatsHistoryOption> options, Func<StatsHistoryOption, decimal> selector, decimal value)
        {
            double index = options.FindIndex(o => selector(o) >= value);
            double percentil = index == -1 ? 0 : 1 - index / (options.Count - 1);
            return Math.Round(percentil, 2);
        }
    }
}
