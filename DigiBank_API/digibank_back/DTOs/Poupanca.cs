using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.DTOs
{
    public class Poupanca
    {
        public int IdPoupanca { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal Saldo { get; set; }
        public decimal GanhoDiario { get; set; }
        public decimal GanhoMensal { get; set; }
        public decimal GanhoAnual { get; set; }

        public Poupanca(int idUsuario)
        {
            digiBankContext ctx = new digiBankContext();
            PoupancaRepository poupancaRepository = new PoupancaRepository();
            List<Investimento> depositos = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOption == 1 &&
                D.IsEntrada)
                .ToList();

            List<Investimento> saques = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOption == 1 &&
                D.IsEntrada == false)
                .ToList();

            if (depositos.Count > 0)
            {
                double jurosPoupanca = (double)ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == 1).PercentualDividendos / 100;
                IdPoupanca = depositos.OrderBy(D => D.DataAquisicao).First().IdInvestimento; //Cada usuario tem apenas uma poupanca
                IdUsuario = idUsuario;
                DataAquisicao = depositos.OrderBy(D => D.DataAquisicao).First().DataAquisicao;
                TotalInvestido = depositos.Sum(D => D.DepositoInicial); //Total investido é a soma de todos os depósitos
                Saldo = poupancaRepository.Saldo(idUsuario, DateTime.Now);
            }
        }
    }
}
