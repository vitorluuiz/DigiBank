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

        public bool Comprar(Investimento newInvestimento) //Averiguar
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
                .Where(I => I.IdUsuario == idUsuario)
                .Include(I => I.IdInvestimentoOptionNavigation.IdAreaInvestimentoNavigation)
                .Include(I => I.IdInvestimentoOptionNavigation.IdTipoInvestimentoNavigation)
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

        public ExtratoInvestimentos ExtratoTotalInvestido(int idUsuario)
        {
            RendaFixaRepository rendaFixaRepository = new RendaFixaRepository();

            decimal saldoPoupanca = new Poupanca(idUsuario).Saldo;
            decimal saldoRendaFixa = rendaFixaRepository.Saldo(idUsuario, DateTime.Now);

            return new ExtratoInvestimentos
            {
                IdUsuario = idUsuario,
                Horario = DateTime.Now,
                Total = ValorInvestimentos(idUsuario, DateTime.Now) + saldoPoupanca,
                Poupanca = saldoPoupanca,
                RendaFixa = saldoRendaFixa,
                Acoes = ValorInvestimento(idUsuario, 3),
                Fundos = ValorInvestimento(idUsuario, 4),
                Criptomoedas = ValorInvestimento(idUsuario, 5)
            };
        }

        public decimal ValorInvestimento(int idUsuario, int idTipoInvestimento)
        {
            List<Investimento> investimentos = ctx.Investimentos
                .Where(I => I.IdUsuario == idUsuario && I.IsEntrada)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            return investimentos
                .Where(I => I.IdInvestimentoOptionNavigation.IdTipoInvestimento == idTipoInvestimento)
                .Sum(I => I.QntCotas * I.IdInvestimentoOptionNavigation.ValorAcao);
        }

        public decimal ValorInvestimentos(int idUsuario, DateTime data)
        {
            HistoryInvestRepository historyInvestRepository = new HistoryInvestRepository();

            List<Investimento> investimentos = ctx.Investimentos
                    .Where(I => I.IdUsuario == idUsuario &&
                    I.IdInvestimentoOptionNavigation.IdTipoInvestimento != 1 &&
                    I.IdInvestimentoOptionNavigation.IdTipoInvestimento != 2)
                    .ToList();

            List<Investimento> depositos = investimentos.Where(I => I.IsEntrada).ToList();
            List<Investimento> saques = investimentos.Where(I => I.IsEntrada == false).ToList();

            decimal valor = 0;
            foreach (Investimento deposito in depositos)
            {
                historyInvestRepository.UpdateOptionHistory(deposito.IdInvestimentoOption);
                decimal valorCota = historyInvestRepository.GetOptionValue(deposito.IdInvestimentoOption, data);

                valor += deposito.QntCotas * valorCota;
            }
            foreach (Investimento saque in saques)
            {
                historyInvestRepository.UpdateOptionHistory(saque.IdInvestimentoOption);
                decimal valorCota = historyInvestRepository.GetOptionValue(saque.IdInvestimentoOption, data);

                valor -= saque.QntCotas * valorCota;
            }

            return valor;
        }
    }
}
