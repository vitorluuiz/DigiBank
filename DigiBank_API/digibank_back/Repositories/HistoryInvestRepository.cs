using Bogus;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class HistoryInvestRepository : IHistoryInvestRepository
    {
        digiBankContext ctx = new digiBankContext();

        public List<HistoricoInvestimentoOption> GetHistoryFromOption(int idOption, int days)
        {
            int ticks = days * 24;
            UpdateOptionHistory(idOption);
            return ctx.HistoricoInvestimentoOptions
                .Where(H => H.IdInvestimentoOption == idOption)
                .OrderBy(H => H.DataH)
                .Take(ticks)
                .AsNoTracking()
                .ToList();
        }

        public void UpdateOptionHistory(int idOption)
        {
            InvestimentoOption option = ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == idOption);

            if(option != null)
            {
                List<HistoricoInvestimentoOption> history = new List<HistoricoInvestimentoOption>();
                TimeSpan spanTime = DateTime.Now - option.Tick;
                int ticks = (int)Math.Round(spanTime.TotalHours);
                Random random = new Random();
                decimal valorAtual = option.ValorAcao;
                decimal qntCotasDisponiveis = (option.QntCotasTotais - ctx.Investimentos.Where(I => I.IdInvestimentoOption == idOption).Sum(I => I.QntCotas));

                for (int i = 0; i < ticks; i++)
                {
                    if(random.Next(1, 3) == 1)
                    {
                        valorAtual = valorAtual + random.Next(1, 3) * -1;
                    }
                    else
                    {
                        valorAtual = valorAtual + random.Next(1, 3);
                    }

                    history.Add(new HistoricoInvestimentoOption
                    {
                        IdInvestimentoOption = (short)idOption,
                        Valor = valorAtual,
                        Cotas = qntCotasDisponiveis,
                        DataH = option.Tick.AddHours(i),
                    });
                    valorAtual = history[i].Valor;
                }

                option.ValorAcao = valorAtual;
                option.Tick = DateTime.Now;

                ctx.Update(option);

                ctx.HistoricoInvestimentoOptions.AddRange(history);
                ctx.SaveChanges();
            }
        }

        ////public IEnumerable<IGrouping<int, HistoricoTotalInvestido>> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim)
        public List<HistoricoTotalInvestido> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim)
        {
            List<HistoricoTotalInvestido> investimentoHitory = new List<HistoricoTotalInvestido>();
            if (inicio > fim) return null; //Alterar para investimentoHistory

            InvestimentoRepository investimentoRepository = new InvestimentoRepository();
            PoupancaRepository poupancaRepository = new PoupancaRepository();
            RendaFixaRepository rendaFixaRepository = new RendaFixaRepository();

            int ticks = (int)Math.Round((fim.AddDays(1) - inicio).TotalDays);

            DateTime today = inicio;
            decimal saldo = 0;
            for (int index = 0; index < ticks; index++)
            {
                saldo = poupancaRepository.Saldo(idUsuario, today);
                saldo += rendaFixaRepository.Saldo(idUsuario, today);
                saldo += investimentoRepository.ValorInvestimentos(idUsuario, today);

                investimentoHitory.Add(new HistoricoTotalInvestido
                {
                    Data = today.ToString("d"),
                    Valor = Math.Round(saldo, 2)
                });

                today = today.AddDays(1);
            }

            investimentoHitory.OrderBy(I => I.Data);
            return investimentoHitory;
        }

        public decimal GetOptionValue(int idOption, DateTime data)
        {
            HistoricoInvestimentoOption history = ctx.HistoricoInvestimentoOptions
                .Where(H => H.DataH.Day == data.Day &&
                H.DataH.Month == data.Month &&
                H.DataH.Year == data.Year)
                .OrderByDescending(H => H.DataH)
                .LastOrDefault();

            return history != null ? history.Valor : 0;
        }
    }
}
