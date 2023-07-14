using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class RendaFixaRepository : IRendaFixaRepository
    {
        digiBankContext ctx = new digiBankContext();

        public decimal CalcularLucros(int idUsuario, DateTime inicio, DateTime fim)
        {
            if (inicio > fim)
            {
                return 0;
            }

            List<Investimento> depositos = ctx.Investimentos
                    .Where(I => I.IdUsuario == idUsuario &&
                    I.IsEntrada &&
                    I.IdInvestimentoOptionNavigation.IdTipoInvestimento == 2 &&
                    I.DataAquisicao < fim)
                    .Include(I => I.IdInvestimentoOptionNavigation)
                    .ToList();

            List<Investimento> saques = ctx.Investimentos
               .Where(I => I.IdUsuario == idUsuario &&
               I.IsEntrada == false &&
               I.IdInvestimentoOptionNavigation.IdTipoInvestimento == 2 &&
               I.DataAquisicao < fim)
               .Include(I => I.IdInvestimentoOptionNavigation)
               .ToList();

            decimal ganhos = 0;
            decimal CalcJuros(decimal juros) { return 1 + juros; }

            foreach (Investimento deposito in depositos)
            {
                decimal timeSpan = 0;
                decimal juros = (decimal)deposito.IdInvestimentoOptionNavigation.PercentualDividendos / 100;

                if (deposito.DataAquisicao > inicio)
                {
                    timeSpan = (decimal)(fim - deposito.DataAquisicao).TotalDays / 30;
                    ganhos += deposito.DepositoInicial * CalcJuros(juros) * timeSpan - deposito.DepositoInicial;
                }
                else
                {
                    timeSpan = (decimal)(fim - inicio).TotalDays / 30;
                    decimal saldoAnterior = deposito.DepositoInicial * CalcJuros(juros) * (decimal)((inicio - deposito.DataAquisicao).TotalDays / 30);
                    ganhos += saldoAnterior * CalcJuros(juros) * timeSpan - saldoAnterior;
                }
            }
            foreach (Investimento saque in saques)
            {
                decimal timeSpan = 0;
                decimal juros = (decimal)saque.IdInvestimentoOptionNavigation.PercentualDividendos;

                if (saque.DataAquisicao > inicio)
                {
                    timeSpan = (decimal)(fim - saque.DataAquisicao).TotalDays / 30;
                    ganhos -= saque.DepositoInicial * CalcJuros(juros) * timeSpan - saque.DepositoInicial;
                }
                else
                {
                    timeSpan = (decimal)(fim - inicio).TotalDays / 30;
                    decimal saldoAnterior = saque.DepositoInicial * CalcJuros(juros) * (decimal)((inicio - saque.DataAquisicao).TotalDays / 30);
                    ganhos -= saldoAnterior * CalcJuros(juros) * timeSpan - saldoAnterior;
                }
            }
            return ganhos;
        }

        public decimal Saldo(int idUsuario, DateTime inicio, DateTime fim)
        {
            if (inicio > fim)
            {
                return 0;
            }

            List<Investimento> depositos = ctx.Investimentos
                    .Where(I => I.IdUsuario == idUsuario &&
                    I.IsEntrada &&
                    I.IdInvestimentoOptionNavigation.IdTipoInvestimento == 2 &&
                    I.DataAquisicao < fim)
                    .Include(I => I.IdInvestimentoOptionNavigation)
                    .ToList();

            List<Investimento> saques = ctx.Investimentos
               .Where(I => I.IdUsuario == idUsuario &&
               I.IsEntrada == false &&
               I.IdInvestimentoOptionNavigation.IdTipoInvestimento == 2 &&
               I.DataAquisicao < fim)
               .Include(I => I.IdInvestimentoOptionNavigation)
               .ToList();

            decimal saldo = 0;
            decimal CalcJuros(decimal juros) { return 1 + juros; }

            foreach (Investimento deposito in depositos)
            {
                decimal timeSpan = 0;
                decimal juros = (decimal)deposito.IdInvestimentoOptionNavigation.PercentualDividendos / 100;

                if (deposito.DataAquisicao > inicio)
                {
                    timeSpan = (decimal)(fim - deposito.DataAquisicao).TotalDays / 30;
                    saldo += deposito.DepositoInicial * CalcJuros(juros) * timeSpan;
                }
                else
                {
                    timeSpan = (decimal)(fim - inicio).TotalDays / 30;
                    decimal saldoAnterior = deposito.DepositoInicial * CalcJuros(juros) * (decimal)((inicio - deposito.DataAquisicao).TotalDays / 30);
                    saldo += saldoAnterior * CalcJuros(juros) * timeSpan;
                }
            }
            foreach (Investimento saque in saques)
            {
                decimal timeSpan = 0;
                decimal juros = (decimal)saque.IdInvestimentoOptionNavigation.PercentualDividendos;

                if (saque.DataAquisicao > inicio)
                {
                    timeSpan = (decimal)(fim - saque.DataAquisicao).TotalDays / 30;
                    saldo -= saque.DepositoInicial * CalcJuros(juros) * timeSpan;
                }
                else
                {
                    timeSpan = (decimal)(fim - inicio).TotalDays / 30;
                    decimal saldoAnterior = saque.DepositoInicial * CalcJuros(juros) * (decimal)((inicio - saque.DataAquisicao).TotalDays / 30);
                    saldo -= saldoAnterior * CalcJuros(juros) * timeSpan;
                }
            }
            return saldo;
        }
    }
}
