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
                .OrderByDescending(H => H.DataHistorico)
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
                        ValorAcao = valorAtual,
                        QntCotasDisponiveis = qntCotasDisponiveis,
                        DataHistorico = option.Tick.AddHours(i),
                    });
                    valorAtual = history[i].ValorAcao;
                }

                option.ValorAcao = valorAtual;
                option.Tick = DateTime.Now;

                ctx.Update(option);

                ctx.HistoricoInvestimentoOptions.AddRange(history);
                ctx.SaveChanges();
            }
        }

        public List<HistoricoTotalInvestido> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim)
        {
            List<HistoricoTotalInvestido> investimentoHitory = new List<HistoricoTotalInvestido>();
            if (inicio > fim) return investimentoHitory;

            PoupancaRepository poupancaRepository = new PoupancaRepository();
            RendaFixaRepository rendaFixaRepository = new RendaFixaRepository();

            int ticks = (int)Math.Round((fim - inicio).TotalHours);

            List<Investimento> entradas = ctx.Investimentos
                .Where(I => I.IdUsuario == idUsuario &&
                I.IsEntrada &&
                I.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            List<Investimento> saidas = ctx.Investimentos
                .Where(I => I.IdUsuario == idUsuario &&
                I.IsEntrada == false &&
                I.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            DateTime today = inicio;
            decimal saldo = 0;
            for (int index = 0; index < ticks; index++)
            {
                saldo = poupancaRepository.Saldo(idUsuario, today.AddHours(-1), today);
                saldo += rendaFixaRepository.Saldo(idUsuario, today.AddHours(-1), today);

                investimentoHitory.Add(new HistoricoTotalInvestido
                {
                    IdHistorico = index,
                    Data = today,
                    Valor = saldo
                });

                today = today.AddHours(1);
            }

            investimentoHitory.OrderByDescending(I => I.Data);
            return investimentoHitory;
        }
    }
}
