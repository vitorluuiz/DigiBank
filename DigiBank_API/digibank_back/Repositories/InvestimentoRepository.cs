using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        public bool Comprar(Investimento newInvestimento)
        {
            var option = _optionsRepository.ListarPorId(newInvestimento.IdInvestimentoOption);
            var _transacaoRepository = new TransacaoRepository(_ctx, _memoryCache);

            if (option == null)
            {
                return false;
            }

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Investimento: {newInvestimento.QntCotas}{(newInvestimento.QntCotas >= 2 ? " cotas" : " Cota")} de {option.Nome}",
                Valor = newInvestimento.QntCotas * option.Valor,
                IdUsuarioPagante = newInvestimento.IdUsuario,
                IdUsuarioRecebente = 1
            };

            newInvestimento.DepositoInicial = newInvestimento.QntCotas * option.Valor;

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if (isSucess)
            {
                Post(newInvestimento, DateTime.Now);
                return true;
            }

            return false;
        }

        public Investimento ListarPorId(int idInvestimento)
        {
            return _ctx.Investimentos
                .Include(f => f.IdInvestimentoOptionNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdInvestimento == idInvestimento);
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
            HistoryInvestRepository historyInvestRepository = new(_ctx, _memoryCache);
            List<Investimento> investimentos = _ctx.Investimentos
                .Where(I => I.IdUsuario == idUsuario &&
                I.IdInvestimentoOptionNavigation.IdTipoInvestimento == idTipoInvestimento &&
                I.DataAquisicao < data)
                .ToList();

            List<Investimento> depositos = investimentos.Where(d => d.IsEntrada).ToList();
            List<Investimento> saques = investimentos.Where(d => d.IsEntrada == false).ToList();

            decimal valor = 0;

            foreach (var deposito in depositos)
            {
                decimal valorAcao = historyInvestRepository.GetOptionValue(deposito.IdInvestimentoOption, data);
                valor = valor + deposito.QntCotas * valorAcao;
            }
            foreach (var saque in saques)
            {
                decimal valorAcao = historyInvestRepository.GetOptionValue(saque.IdInvestimentoOption, data);
                valor = valor + saque.QntCotas * valorAcao;
            }

            return valor;
        }

        public void Post(Investimento newInvestimento, DateTime date)
        {
            if (newInvestimento == null)
            {
                throw new Exception("Investimento não pode ser nulo");
            }

            newInvestimento.IsEntrada = true;
            newInvestimento.DataAquisicao = date;

            _ctx.Investimentos.Add(newInvestimento);
            _ctx.SaveChanges();
        }

        public List<Investimento> AllWhere(Expression<Func<Investimento, bool>> where, int pagina, int qntItens)
        {
            return _ctx.Investimentos
                .Where(where)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .Include(I => I.IdInvestimentoOptionNavigation.IdAreaInvestimentoNavigation)
                .Include(I => I.IdInvestimentoOptionNavigation.IdTipoInvestimentoNavigation)
                .AsNoTracking()
                .ToList();
        }

        public int CountWhere(Expression<Func<Investimento, bool>> where)
        {
            return _ctx.Investimentos.Count(where);
        }
    }
}
