using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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

        public bool VenderCotas(int idUsuario, int idOption, decimal qntCotas)
        {
            if (qntCotas <= 0)
            {
                throw new Exception("Número de cotas não aceito");
            }

            InvestimentoGenerico investimento = GetCarteiraItem(idUsuario, idOption);

            if(investimento == null)
            {
                throw new Exception("Investimento Option ativa não encontrada");
            }

            TransacaoRepository _transacaoRepository = new(_ctx, _memoryCache);
            decimal valor = investimento.IdInvestimentoOptionNavigation.ValorAcao * qntCotas;
            DateTime date = DateTime.Now;

            if (qntCotas > investimento.QntCotas)
            {
                throw new Exception("Não há cotas proprietárias para venda");
            }

            Transaco transacao = new Transaco
            {
                DataTransacao = date,
                Descricao = $"Venda investimento: {qntCotas}{(qntCotas >= 2 ? "Cotas" : "Cota")}",
                Valor = valor,
                IdUsuarioPagante = 1,
                IdUsuarioRecebente = idUsuario,
            };

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if(!isSucess)
            {
                return false;
            }

            var saque = new Investimento
            {
                IdUsuario = idUsuario,
                IdInvestimentoOption = (short)idOption,
                DataAquisicao = date,
                IsEntrada = false,
                DepositoInicial = valor,
                QntCotas = qntCotas,
            };

            _ctx.Investimentos.Add(saque);
            _ctx.SaveChanges();

            return isSucess;
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

        public List<InvestimentoGenerico> AllWhere(Expression<Func<Investimento, bool>> where, int pagina, int qntItens)
        {
            var list = _ctx.Investimentos
                .Where(where)
                .OrderBy(i => i.IdInvestimentoOption)
                .Include(I => I.IdInvestimentoOptionNavigation)
                .AsEnumerable()
                .GroupBy(i => i.IdInvestimentoOption)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .Select(group => new List<Investimento>(group))
                .ToList();                
            
            return list.Select(group => new InvestimentoGenerico(group))
                .ToList();
        }

        public int CountWhere(Expression<Func<Investimento, bool>> where)
        {
            return _ctx.Investimentos
                .Where(where)
                .GroupBy(i => i.IdInvestimentoOption)
                .Count();
        }

        public List<InvestimentoGenerico> GetCarteira(int idUsuario, int idTipoInvestimento, int pagina, int qntItens)
        {
            List<InvestimentoGenerico> depositos = AllWhere(i => i.IdUsuario == idUsuario && i.IsEntrada, pagina, qntItens);
            List<InvestimentoGenerico> carteiraList = new();

            foreach (InvestimentoGenerico deposito in depositos)
            {
                List<InvestimentoGenerico> saques = AllWhere(i => i.IdUsuario == idUsuario
                && i.IsEntrada == false
                && i.IdInvestimentoOption == deposito.IdInvestimentoOption, 1, 1);

                if (saques.Count == 1)
                {
                    InvestimentoGenerico saque = saques.First();

                    carteiraList.Add(new InvestimentoGenerico
                    {
                        IdUsuario = idUsuario,
                        IdInvestimentoOption = deposito.IdInvestimentoOption,
                        DataAquisicao = deposito.DataAquisicao,
                        IdInvestimento = deposito.IdInvestimento,
                        DepositoInicial = deposito.DepositoInicial - saque.DepositoInicial,
                        IdInvestimentoOptionNavigation = deposito.IdInvestimentoOptionNavigation,
                        QntCotas = deposito.QntCotas - saque.QntCotas,
                    });
                } else
                {
                    carteiraList.Add(new InvestimentoGenerico
                    {
                        IdUsuario = idUsuario,
                        IdInvestimentoOption = deposito.IdInvestimentoOption,
                        DataAquisicao = deposito.DataAquisicao,
                        IdInvestimento = deposito.IdInvestimento,
                        DepositoInicial = deposito.DepositoInicial,
                        IdInvestimentoOptionNavigation = deposito.IdInvestimentoOptionNavigation,
                        QntCotas = deposito.QntCotas,
                    });
                }
            }

            return carteiraList.Where(i => i.QntCotas > 0).ToList();
        }

        public InvestimentoGenerico GetCarteiraItem(int idUsuario, int idOption)
        {
            List<InvestimentoGenerico> depositos = AllWhere(i => i.IdUsuario == idUsuario
            && i.IdInvestimentoOption == idOption
            && i.IsEntrada, 1, 1);

            if (depositos.Count != 1)
            {
                return null;
            }

            InvestimentoGenerico deposito = depositos.First();

            List<InvestimentoGenerico> saques = AllWhere(i => i.IdUsuario == idUsuario
                && i.IsEntrada == false
                && i.IdInvestimentoOption == idOption, 1, 1);

            if (saques.Count != 1)
            {
                return deposito;
            }
            
            InvestimentoGenerico saque = saques.First();

            if (deposito.QntCotas <= saque.QntCotas)
            {
                return null;
            }

            return new InvestimentoGenerico
            {
                IdInvestimento = deposito.IdInvestimentoOption,
                IdUsuario = idUsuario,
                IdInvestimentoOption = deposito.IdInvestimentoOption,
                DataAquisicao = deposito.DataAquisicao,
                DepositoInicial = deposito.DepositoInicial - saque.DepositoInicial,
                QntCotas = deposito.QntCotas - saque.QntCotas,
                IdInvestimentoOptionNavigation = deposito.IdInvestimentoOptionNavigation,
                IsEntrada = true
            };
        }
    }
}
