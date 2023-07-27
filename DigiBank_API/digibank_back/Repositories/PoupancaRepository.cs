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
    public class PoupancaRepository : IPoupancaRepository
    {
        private readonly digiBankContext _ctx;
        private readonly IMemoryCache _memoryCache;

        public PoupancaRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _memoryCache = memoryCache;
        }

        public decimal CalcularLucro(int idUsuario, DateTime inicio, DateTime fim)
        {
            if (inicio > fim)
            {
                return 0;
            }

            List<Investimento> investimentos = _ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 1 &&
                D.DataAquisicao < fim)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            var depositos = investimentos.Where(d => d.IsEntrada);
            var saques = investimentos.Where(s => s.IsEntrada == false);

            decimal ganhos = 0;
            decimal jurosPoupanca = _ctx.InvestimentoOptions.FirstOrDefault(i => i.IdInvestimentoOption == 1).PercentualDividendos / 100;
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
                        (decimal)Math.Pow(CalcJuros(jurosPoupanca), (inicio - deposito.DataAquisicao).TotalDays / 30);
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
                        (decimal)Math.Pow(CalcJuros(jurosPoupanca), (inicio - saque.DataAquisicao).TotalDays / 30);
                    ganhos -= (saldoAnterior * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan)) - saldoAnterior;
                }
            }

            return ganhos;
        }

        public bool Depositar(int idUsuario, decimal quantidade)
        {
            Usuario usuario = _ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == idUsuario);
            if (usuario == null) { return false; }
            UsuarioRepository usuarioRepository = new UsuarioRepository(_ctx, _memoryCache);

            if (usuarioRepository.CanRemoveSaldo(idUsuario, quantidade))
            {
                usuarioRepository.RemoverSaldo((short)idUsuario, quantidade);
                _ctx.Investimentos.Add(new Investimento
                {
                    IdInvestimentoOption = 1,
                    IdUsuario = idUsuario,
                    DepositoInicial = quantidade,
                    DataAquisicao = DateTime.Now,
                    IsEntrada = true,
                    QntCotas = 1,
                });
                _ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Sacar(int idUsuario, decimal quantidade)
        {
            UsuarioRepository _usuarioRepository = new UsuarioRepository(_ctx, _memoryCache);
            Usuario usuario = _ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == idUsuario);
            Poupanca poupanca = new Poupanca(idUsuario, _ctx, _memoryCache);

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

                _ctx.Investimentos.Add(saque);
                _ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public decimal Saldo(int idUsuario, DateTime data)
        {
            List<Investimento> investimentos = _ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 1 &&
                D.DataAquisicao < data)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            var depositos = investimentos.Where(I => I.IsEntrada).ToList();
            var saques = investimentos.Where(I => I.IsEntrada == false).ToList();

            if (depositos.Count == 0) return 0;

            decimal saldo = 0;
            decimal jurosPoupanca = depositos[0].IdInvestimentoOptionNavigation.PercentualDividendos / 100;
            double CalcJuros(decimal juros) { return (double)(1 + juros); }

            foreach (Investimento deposito in depositos)
            {
                var timeSpan = (data - deposito.DataAquisicao).TotalDays / 30;
                saldo += deposito.DepositoInicial * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan);
            }
            foreach (Investimento saque in saques)
            {
                var timeSpan = (data - saque.DataAquisicao).TotalDays / 30;
                saldo -= saque.DepositoInicial * (decimal)Math.Pow(CalcJuros(jurosPoupanca), timeSpan);
            }

            return saldo;
        }
    }
}
