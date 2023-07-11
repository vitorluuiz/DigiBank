using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.DTOs
{
    public class Poupanca
    {
        public int IdPoupanca { get; set; }
        public int IdInvestimentoOption { get; set; }
        public int IdUsuario { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal Saldo { get; set; }
        public decimal GanhoDiario { get; set; }
        public decimal GanhoMensal { get; set; }
        public decimal GanhoAnual { get; set; }

        public Poupanca(int idUsuario)
        {
            digiBankContext ctx = new digiBankContext();
            List<Investimento> depositos = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOption == 1)
                .ToList();

            if (depositos.Count > 0)
            {
                decimal jurosPoupanca = (decimal)ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == 1).PercentualDividendos;
                IdPoupanca = depositos.OrderBy(D => D.DataAquisicao).First().IdInvestimento; //Cada usuario tem apenas uma poupanca
                IdUsuario = idUsuario;
                TotalInvestido = depositos.Sum(D => D.DepositoInicial); //Total investido é a soma de todos os depósitos
                Saldo = depositos
                    .Sum(D => D.DepositoInicial + //Soma do produto de Valor depositado
                    D.DepositoInicial * jurosPoupanca / 100 * //Percentual da poupanca (Sem verificar alteracoes)
                    Convert.ToDecimal((DateTime.Now - D.DataAquisicao).TotalDays / 30)); //Total de meses
            }
        }
    }
}
