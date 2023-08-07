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
    public class InvestimentoRepository : IInvestimentoRepository
    {
        private readonly digiBankContext _ctx;
        private readonly IInvestimentoOptionsRepository _optionsRepository;
        private readonly IMemoryCache _memoryCache;
        public InvestimentoRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _optionsRepository = new InvestimentoOptionsRepository(ctx, memoryCache);
            _memoryCache = memoryCache;
        }


        public bool Comprar(Investimento newInvestimento) //Averiguar
        {
            InvestimentoOptionGenerico option = _optionsRepository.ListarPorId(newInvestimento.IdInvestimentoOption);
            TransacaoRepository _transacaoRepository = new TransacaoRepository(_ctx, _memoryCache);

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Aquisição investimento de {newInvestimento.QntCotas}{(newInvestimento.QntCotas > 1 ? " cotas" : " Cota")} de {option.Nome}",
                Valor = newInvestimento.QntCotas * option.Valor,
                IdUsuarioPagante = newInvestimento.IdUsuario,
                IdUsuarioRecebente = 1
            };

            if (option == null)
            {
                return false;
            }

            newInvestimento.DepositoInicial = newInvestimento.QntCotas * option.Valor;
            newInvestimento.DataAquisicao = DateTime.Now;

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if (isSucess)
            {
                _ctx.Investimentos.Add(newInvestimento);
                _ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public List<Investimento> ListarDeUsuario(int idUsuario, int pagina, int qntItens)
        {
            return _ctx.Investimentos
                .Where(I => I.IdUsuario == idUsuario)
                .Include(I => I.IdInvestimentoOptionNavigation.IdAreaInvestimentoNavigation)
                .Include(I => I.IdInvestimentoOptionNavigation.IdTipoInvestimentoNavigation)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }

        public Investimento ListarPorId(int idInvestimento)
        {
            return _ctx.Investimentos
                .Include(f => f.IdInvestimentoOptionNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdInvestimento == idInvestimento);
        }

        public List<Investimento> ListarTodos()
        {
            return _ctx.Investimentos
                .AsNoTracking()
                .ToList();
        }

        public void Vender(int idInvestimento)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository(_ctx, _memoryCache);
            Investimento investimentoVendido = ListarPorId(idInvestimento);
            TimeSpan diasInvestidos = investimentoVendido.DataAquisicao - DateTime.Now;
            decimal valorGanho = (decimal)(investimentoVendido.DepositoInicial + (investimentoVendido.DepositoInicial * (Convert.ToInt16(diasInvestidos.TotalDays / 30)) * (investimentoVendido.IdInvestimentoOptionNavigation.PercentualDividendos / 100)));

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Venda investimento de {investimentoVendido.QntCotas}{(investimentoVendido.QntCotas > 0 ? "Cotas" : "Cota")}",
                Valor = valorGanho,
                IdUsuarioPagante = 1,
                IdUsuarioRecebente = investimentoVendido.IdUsuario
            };

            _transacaoRepository.EfetuarTransacao(transacao);

            _ctx.Investimentos.Remove(investimentoVendido);
            _ctx.SaveChanges();
        }

        public void VenderCotas(int idInvestimento, decimal qntCotas)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository(_ctx, _memoryCache);
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

            _ctx.Update(investimentoVendido);
            _ctx.SaveChanges();
        }

        public ExtratoInvestimentos ExtratoTotalInvestido(int idUsuario)
        {
            RendaFixaRepository rendaFixaRepository = new RendaFixaRepository();
            DateTime now = DateTime.Now;

            decimal saldoPoupanca = new Poupanca(idUsuario, _ctx, _memoryCache).Saldo;
            decimal saldoRendaFixa = rendaFixaRepository.Saldo(idUsuario, now);
            decimal saldoAcoes = ValorInvestimento(idUsuario, 3, now);
            decimal saldoFundos = ValorInvestimento(idUsuario, 4, now);
            decimal saldoCripto = ValorInvestimento(idUsuario, 5, now);

            return new ExtratoInvestimentos
            {
                IdUsuario = idUsuario,
                Horario = DateTime.Now,
                Total = saldoPoupanca + saldoRendaFixa + saldoAcoes + saldoFundos + saldoCripto,
                Poupanca = saldoPoupanca,
                RendaFixa = saldoRendaFixa,
                Acoes = saldoAcoes,
                Fundos = saldoFundos,
                Criptomoedas = saldoCripto
            };
        }

        public decimal ValorInvestimento(int idUsuario, int idTipoInvestimento, DateTime data)
        {
            HistoryInvestRepository historyInvestRepository = new HistoryInvestRepository(_ctx, _memoryCache);
            List<Investimento> investimentos = _ctx.Investimentos
                .Where(I => I.IdUsuario == idUsuario &&
                I.IdInvestimentoOptionNavigation.IdTipoInvestimento == idTipoInvestimento &&
                I.DataAquisicao < data)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .ToList();

            List<Investimento> depositos = investimentos.Where(d => d.IsEntrada).ToList();
            List<Investimento> saques = investimentos.Where(d => d.IsEntrada == false).ToList();

            decimal valor = 0;
            foreach (var deposito in depositos)
            {
                historyInvestRepository.UpdateOptionHistory(deposito.IdInvestimentoOption);
                valor = +deposito.QntCotas * deposito.IdInvestimentoOptionNavigation.ValorAcao;
            }
            foreach (var saque in saques)
            {
                historyInvestRepository.UpdateOptionHistory(saque.IdInvestimentoOption);
                valor = +saque.QntCotas * saque.IdInvestimentoOptionNavigation.ValorAcao;
            }

            return valor;
        }
    }
}
