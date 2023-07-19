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
    public class PoupancaRepository : IPoupancaRepository
    {
        digiBankContext ctx = new digiBankContext();
        public decimal CalcularLucro(int idUsuario, DateTime inicio, DateTime fim)
        {
            if (inicio > fim)
            {
                return 0;
            }

            List<Investimento> depositos = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 1 &&
                D.IsEntrada &&
                D.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            List<Investimento> saques = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 1 &&
                D.IsEntrada == false &&
                D.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            decimal ganhos = 0;
            decimal jurosPoupanca = (decimal)(depositos[0].IdInvestimentoOptionNavigation.PercentualDividendos / 100);
            double CalcJuros(decimal juros) { return (double)(1 + juros); }

            foreach (Investimento deposito in depositos)
            {
                double timeSpan = 0;

                if (deposito.DataAquisicao > inicio)
                {
                    timeSpan = (fim - deposito.DataAquisicao).TotalDays / 30;
                    ganhos += deposito.DepositoInicial * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan) - deposito.DepositoInicial;
                }
                else
                {
                    timeSpan = (fim - inicio).TotalDays / 30;
                    decimal saldoAnterior = deposito.DepositoInicial *
                        (decimal)Math.Pow(CalcJuros(jurosPoupanca), (double)(inicio - deposito.DataAquisicao).TotalDays / 30);
                    ganhos += (saldoAnterior * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan)) - saldoAnterior;
                }
            }
            foreach (Investimento saque in saques)
            {
                double timeSpan = 0;

                if (saque.DataAquisicao > inicio)
                {
                    timeSpan = (fim - saque.DataAquisicao).TotalDays / 30;
                    ganhos -= saque.DepositoInicial * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan) - saque.DepositoInicial;
                }
                else
                {
                    timeSpan = (fim - inicio).TotalDays / 30;
                    decimal saldoAnterior = saque.DepositoInicial *
                        (decimal)Math.Pow(CalcJuros(jurosPoupanca), (double)(inicio - saque.DataAquisicao).TotalDays / 30);
                    ganhos -= (saldoAnterior * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan)) - saldoAnterior;
                }
            }

            return ganhos;
        }

        public bool Depositar(int idUsuario, decimal quantidade)
        {
            Usuario usuario = ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == idUsuario);
            if (usuario == null) { return false; }
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            
            if (usuarioRepository.CanRemoveSaldo(idUsuario, quantidade))
            {
                usuarioRepository.RemoverSaldo((short)idUsuario, quantidade);
                ctx.Investimentos.Add(new Investimento
                {
                    IdInvestimentoOption = 1,
                    IdUsuario = idUsuario,
                    DepositoInicial = quantidade,
                    DataAquisicao = DateTime.Now,
                    IsEntrada = true,
                    QntCotas = 1,
                });
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Sacar(int idUsuario, decimal quantidade)
        {
            UsuarioRepository _usuarioRepository = new UsuarioRepository();
            Usuario usuario = ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == idUsuario);
            Poupanca poupanca = new Poupanca(idUsuario);

            bool canAddSaldo = _usuarioRepository.CanAddSaldo(idUsuario, quantidade);
            bool hasSaldo = poupanca.Saldo >= quantidade;

            if (canAddSaldo && hasSaldo)
            {
                Investimento saque = new Investimento()
                {
                    IdInvestimentoOption = 1,
                    IdUsuario = idUsuario,
                    DepositoInicial = quantidade,
                    DataAquisicao = DateTime.Now,
                    IsEntrada = false,
                    QntCotas = 1
                };

                _usuarioRepository.AdicionarSaldo(idUsuario, quantidade);

                ctx.Investimentos.Add(saque);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public decimal Saldo(int idUsuario, DateTime inicio, DateTime fim)
        {
            List<Investimento> depositos = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 1 &&
                D.IsEntrada &&
                D.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            List<Investimento> saques = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 1 &&
                D.IsEntrada == false &&
                D.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            decimal saldo = 0;
            decimal jurosPoupanca = (decimal)depositos[0].IdInvestimentoOptionNavigation.PercentualDividendos / 100;
            double CalcJuros(decimal juros) { return (double)(1 + jurosPoupanca); }

            foreach (Investimento deposito in depositos)
            {
                double timeSpan = (DateTime.Now - deposito.DataAquisicao).TotalDays / 30;
                saldo += deposito.DepositoInicial * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan);
            }
            foreach (Investimento saque in saques)
            {
                double timeSpan = (DateTime.Now - saque.DataAquisicao).TotalDays / 30;
                saldo -= saque.DepositoInicial * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan);
            }

            return saldo;
        }
    }
}
