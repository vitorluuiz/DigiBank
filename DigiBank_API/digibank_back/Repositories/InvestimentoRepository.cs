﻿using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class InvestimentoRepository : IInvestimentoRepository
    {
        digiBankContext ctx = new digiBankContext();
        private UsuarioRepository _usuarioRepository = new UsuarioRepository();

        public PreviewRentabilidade CalcularGanhos(int idInvestimento)
        {
            Investimento investimento = ListarPorId(idInvestimento);
            PreviewRentabilidade previsao = new PreviewRentabilidade();

            TimeSpan diferenca = DateTime.Now - investimento.DataAquisicao;

            previsao.DepositoInicial = investimento.DepositoInicial;
            previsao.MontanteTotal = investimento.DepositoInicial + (investimento.DepositoInicial * (diferenca.Days / 30) * (investimento.IdInvestimentoOptionNavigation.Dividendos / 100));
            previsao.GanhosPrevistos = previsao.MontanteTotal - previsao.DepositoInicial;
            previsao.TaxaJuros = investimento.IdInvestimentoOptionNavigation.Dividendos;
            previsao.DiasInvestidos = diferenca.Days;

            return previsao;
        }

        public PreviewRentabilidade CalcularPrevisao(int idInvestimento, decimal diasInvestidos)
        {
            Investimento investimento = ListarPorId(idInvestimento);
            PreviewRentabilidade previsao = new PreviewRentabilidade();
            previsao.DepositoInicial = investimento.DepositoInicial;
            previsao.MontanteTotal = investimento.DepositoInicial + (investimento.DepositoInicial * (diasInvestidos / 30) * (investimento.IdInvestimentoOptionNavigation.Dividendos /100));
            previsao.GanhosPrevistos = previsao.MontanteTotal - previsao.DepositoInicial;
            previsao.TaxaJuros = investimento.IdInvestimentoOptionNavigation.Dividendos;
            previsao.DiasInvestidos = diasInvestidos;

            return previsao;
        }

        public bool Comprar(Investimento newInvestimento, int idComprador)
        {
            newInvestimento.DepositoInicial = newInvestimento.QntCotas * newInvestimento.IdInvestimentoOptionNavigation.ValorInicial;
            newInvestimento.DataAquisicao = DateTime.Now;

            bool isSucess = _usuarioRepository.RemoverSaldo(Convert.ToInt16(idComprador), newInvestimento.DepositoInicial);

            if(isSucess )
            {
                ctx.Investimentos.Add(newInvestimento);
                ctx.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Investimento> ListarDeUsuario(int idUsuario)
        {
            return ctx.Investimentos
                .Where(f => f.IdUsuario == idUsuario)
                .Include(f => f.IdInvestimentoOptionNavigation)
                .AsNoTracking()
                .ToList();
        }

        public Investimento ListarPorId(int idInvestimento)
        {
            return ctx.Investimentos
                .Include(f => f.IdInvestimentoOptionNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdInvestimento == idInvestimento);
        }

        public List<Investimento> ListarTodos()
        {
            return ctx.Investimentos
                .AsNoTracking()
                .ToList();
        }

        public void Vender(int idInvestimento, int idVendedor)
        {
            Investimento investimentoVendido = ListarPorId(idInvestimento);
            TimeSpan diasInvestidos = investimentoVendido.DataAquisicao - DateTime.Now;
            decimal valorGanho = investimentoVendido.DepositoInicial + (investimentoVendido.DepositoInicial * (Convert.ToInt16(diasInvestidos.TotalDays /30)) * (investimentoVendido.IdInvestimentoOptionNavigation.Dividendos/100));

            _usuarioRepository.AdicionarSaldo(idVendedor, valorGanho);

            ctx.Investimentos.Remove(investimentoVendido);
            ctx.SaveChanges();
        }

        public void VenderCotas(int idInvestimento, decimal qntCotas)
        {
            Investimento investimentoVendido = ListarPorId(idInvestimento);
            TimeSpan diasInvestidos = investimentoVendido.DataAquisicao - DateTime.Now;

            investimentoVendido.DataAquisicao = DateTime.Now;

            decimal valorGanho = investimentoVendido.DepositoInicial + (investimentoVendido.DepositoInicial * (Convert.ToInt16(diasInvestidos.TotalDays / 30)) * (investimentoVendido.IdInvestimentoOptionNavigation.Dividendos / 100) * investimentoVendido.QntCotas);

            _usuarioRepository.AdicionarSaldo(investimentoVendido.IdUsuario, valorGanho);

            investimentoVendido.QntCotas = investimentoVendido.QntCotas - qntCotas;

            ctx.Update(investimentoVendido);
            ctx.SaveChanges();
        }
    }
}
