using Bogus;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class HistoricoOptionsRepository : IHistoricoOptionsRepository
    {
        digiBankContext ctx = new digiBankContext();

        public List<HistoricoInvestimentoOption> GetHistoryFromOption(int idOption, int qntItens, int pagina)
        {
            return ctx.HistoricoInvestimentoOptions
                .Where(H => H.IdInvestimentoOption == idOption)
                .OrderBy(H => H.DataHistorico)
                .AsNoTracking()
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public void UpdateHistory(int idOption)
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
                        IdInvestimentoOption = (byte)idOption,
                        ValorAcao = valorAtual,
                        QntCotasDisponiveis = (short?)qntCotasDisponiveis,
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
    }
}
