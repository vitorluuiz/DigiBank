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
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly IInvestimentoOptionsRepository _optionsRepository;
        public InvestimentoRepository()
        {
            _optionsRepository = new InvestimentoOptionsRepository();
        }

        digiBankContext ctx = new digiBankContext();

        public PreviewRentabilidade CalcularGanhos(int idInvestimento)
        {
            Investimento investimento = ListarPorId(idInvestimento);
            PreviewRentabilidade previsao = new PreviewRentabilidade();

            TimeSpan diferenca = DateTime.Now - investimento.DataAquisicao;

            previsao.DepositoInicial = investimento.DepositoInicial;
            previsao.MontanteTotal = (decimal)(investimento.DepositoInicial + (investimento.DepositoInicial * (diferenca.Days / 30) * (investimento.IdInvestimentoOptionNavigation.PercentualDividendos / 100)));
            previsao.GanhosPrevistos = previsao.MontanteTotal - previsao.DepositoInicial;
            previsao.TaxaJuros = (decimal)investimento.IdInvestimentoOptionNavigation.PercentualDividendos;
            previsao.DiasInvestidos = diferenca.Days;

            return previsao;
        }

        public PreviewRentabilidade CalcularPrevisao(int idInvestimento, decimal diasInvestidos)
        {
            Investimento investimento = ListarPorId(idInvestimento);
            PreviewRentabilidade previsao = new PreviewRentabilidade();
            previsao.DepositoInicial = investimento.DepositoInicial;
            previsao.MontanteTotal = (decimal)(investimento.DepositoInicial + (investimento.DepositoInicial * (diasInvestidos / 30) * (investimento.IdInvestimentoOptionNavigation.PercentualDividendos /100)));
            previsao.GanhosPrevistos = previsao.MontanteTotal - previsao.DepositoInicial;
            previsao.TaxaJuros = (decimal)investimento.IdInvestimentoOptionNavigation.PercentualDividendos;
            previsao.DiasInvestidos = diasInvestidos;

            return previsao;
        }

        public bool Comprar(Investimento newInvestimento)
        {
            InvestimentoOption option = _optionsRepository.ListarPorId(newInvestimento.IdInvestimentoOption);
            TransacaoRepository _transacaoRepository = new TransacaoRepository();

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Aquisição investimento de {newInvestimento.QntCotas}{(newInvestimento.QntCotas > 1 ? " cotas" : " Cota")} de {newInvestimento.IdInvestimentoOptionNavigation.Nome}",
                Valor = newInvestimento.QntCotas * option.ValorAcao,
                IdUsuarioPagante = newInvestimento.IdUsuario,
                IdUsuarioRecebente = 1
            };

            if(option == null) 
            {
                return false;
            }

            newInvestimento.DepositoInicial = newInvestimento.QntCotas * option.ValorAcao;
            newInvestimento.DataAquisicao = DateTime.Now;

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if(isSucess)
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

        public decimal RetornarInvestimentoTotal(int idUsuario)
        {
            return ctx.Investimentos
                .Where(i => i.IdUsuario == idUsuario)
                .Sum(i => i.DepositoInicial * i.QntCotas);
        }

        public void Vender(int idInvestimento)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Investimento investimentoVendido = ListarPorId(idInvestimento);
            TimeSpan diasInvestidos = investimentoVendido.DataAquisicao - DateTime.Now;
            decimal valorGanho = (decimal)(investimentoVendido.DepositoInicial + (investimentoVendido.DepositoInicial * (Convert.ToInt16(diasInvestidos.TotalDays /30)) * (investimentoVendido.IdInvestimentoOptionNavigation.PercentualDividendos/100)));

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Venda investimento de {investimentoVendido.QntCotas}{(investimentoVendido.QntCotas > 0 ? "Cotas" : "Cota")}",
                Valor = valorGanho,
                IdUsuarioPagante = 1,
                IdUsuarioRecebente = investimentoVendido.IdUsuario
            };

            _transacaoRepository.EfetuarTransacao(transacao);

            ctx.Investimentos.Remove(investimentoVendido);
            ctx.SaveChanges();
        }

        public void VenderCotas(int idInvestimento, decimal qntCotas)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Investimento investimentoVendido = ListarPorId(idInvestimento);
            TimeSpan diasInvestidos = investimentoVendido.DataAquisicao - DateTime.Now;
            decimal valorGanho = (decimal)(investimentoVendido.DepositoInicial + (investimentoVendido.DepositoInicial * (Convert.ToInt16(diasInvestidos.TotalDays / 30)) * (investimentoVendido.IdInvestimentoOptionNavigation.PercentualDividendos / 100) * investimentoVendido.QntCotas));
            investimentoVendido.DataAquisicao = DateTime.Now;

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Venda investimento de {investimentoVendido.QntCotas}{(investimentoVendido.QntCotas > 0 ? "Cotas" : "Cota")}",
                Valor = valorGanho,
                IdUsuarioPagante = 1,
                IdUsuarioRecebente = investimentoVendido.IdUsuario
            };

            _transacaoRepository.EfetuarTransacao(transacao);

            investimentoVendido.QntCotas = investimentoVendido.QntCotas - qntCotas;

            ctx.Update(investimentoVendido);
            ctx.SaveChanges();
        }
    }
}
