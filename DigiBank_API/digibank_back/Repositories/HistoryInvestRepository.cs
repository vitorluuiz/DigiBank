using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class HistoryInvestRepository : IHistoryInvestRepository
    {
        private readonly digiBankContext _ctx;
        private readonly IMemoryCache _memoryCache;
        public HistoryInvestRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _memoryCache = memoryCache;
        }

        public List<HistoricoInvestimentoOption> GetHistoryFromOption(int idOption, int days)
        {
            UpdateOptionHistory(idOption);
            return _ctx.HistoricoInvestimentoOptions
                .Where(H => H.IdInvestimentoOption == idOption &&
                H.IdInvestimentoOptionNavigation.IdTipoInvestimento != 1 &&
                H.IdInvestimentoOptionNavigation.IdTipoInvestimento != 2)
                .OrderByDescending(H => H.DataH)
                .Take(days)
                .AsNoTracking()
                .ToList();
        }

        public void UpdateOptionHistory(int idOption)
        {
            InvestimentoOption option = _ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == idOption);

            if (option != null && option.Tick.AddDays(1) !< DateTime.Now)
            {
                List<HistoricoInvestimentoOption> history = new();
                TimeSpan spanTime = DateTime.Now - option.Tick;
                int ticks = (int)Math.Round(spanTime.TotalDays);
                Random random = new Random();
                decimal valorAtual = option.ValorAcao;

                for (int i = 0; i < ticks; i++)
                {
                    if (random.Next(1, 3) == 1)
                    {
                        valorAtual = valorAtual + (valorAtual / 100) * (decimal)random.NextDouble() * -1;
                    }
                    else
                    {
                        valorAtual = valorAtual + (valorAtual / 100) * (decimal)random.NextDouble();
                    }

                    history.Add(new HistoricoInvestimentoOption
                    {
                        IdInvestimentoOption = (short)idOption,
                        Valor = valorAtual,
                        DataH = option.Tick.AddDays(i),
                    });
                    valorAtual = history[i].Valor;
                }

                option.ValorAcao = valorAtual;
                option.Tick = DateTime.Now;

                _ctx.Update(option);

                _ctx.HistoricoInvestimentoOptions.AddRange(history);
                _ctx.SaveChanges();
            }
        }

        public List<HistoricoTotalInvestido> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim)
        {
            List<HistoricoTotalInvestido> investimentoHitory = new List<HistoricoTotalInvestido>();
            if (inicio > fim) return investimentoHitory;

            InvestimentoRepository investimentoRepository = new(_ctx, _memoryCache);
            PoupancaRepository poupancaRepository = new(_ctx, _memoryCache);
            RendaFixaRepository rendaFixaRepository = new();

            int ticks = (int)Math.Abs((fim.AddMonths(1) - inicio).TotalDays / 30);

            DateTime today = inicio;
            decimal saldo;
            for (int index = 0; index < ticks; index++)
            {
                saldo = poupancaRepository.Saldo(idUsuario, today);
                saldo += rendaFixaRepository.Saldo(idUsuario, today);
                saldo += investimentoRepository.ValorInvestimento(idUsuario, 3, today);
                saldo += investimentoRepository.ValorInvestimento(idUsuario, 4, today);
                saldo += investimentoRepository.ValorInvestimento(idUsuario, 5, today);

                investimentoHitory.Add(new HistoricoTotalInvestido
                {
                    IdHistorico = index,
                    DataH = today,
                    Valor = Math.Round(saldo, 2)
                });

                today = today.AddMonths(1);
            }

            return investimentoHitory;
        }

        public decimal GetOptionValue(int idOption, DateTime data)
        {
            UpdateOptionHistory(idOption);
            HistoricoInvestimentoOption history = _ctx.HistoricoInvestimentoOptions
                .FirstOrDefault(H => H.IdInvestimentoOption == idOption &&
                H.DataH.Day == data.Day &&
                H.DataH.Month == data.Month &&
                H.DataH.Year == data.Year);

            return history?.Valor ?? 0;
        }
    }
}
