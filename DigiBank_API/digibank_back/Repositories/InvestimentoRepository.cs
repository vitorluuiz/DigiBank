using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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

            InvestimentoGenerico investimento = GetCarteiraItem(o => o.IdUsuario == idUsuario && o.IdInvestimentoOption == idOption);

            if(investimento == null)
            {
                throw new Exception("Investimento Option ativa não encontrada");
            }

            TransacaoRepository _transacaoRepository = new(_ctx, _memoryCache);
            decimal valor = investimento.IdInvestimentoOptionNavigation.Valor * qntCotas;
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
            RendaFixaRepository rendaFixaRepository = new();
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
                I.IdInvestimentoOptionNavigation.IdAreaInvestimentoNavigation.IdTipoInvestimento == idTipoInvestimento &&
                I.DataAquisicao < data)
                .ToList();

            List<Investimento> depositos = investimentos.Where(d => d.IsEntrada).ToList();
            List<Investimento> saques = investimentos.Where(d => d.IsEntrada == false).ToList();

            decimal valor = 0;

            foreach (var deposito in depositos)
            {
                decimal valorAcao = historyInvestRepository.GetOptionValue(deposito.IdInvestimentoOption, data);
                valor += deposito.QntCotas * valorAcao;
            }
            foreach (var saque in saques)
            {
                decimal valorAcao = historyInvestRepository.GetOptionValue(saque.IdInvestimentoOption, data);
                valor -= saque.QntCotas * valorAcao;
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

        public List<InvestimentoGenerico> AllWhere(Expression<Func<Investimento, bool>> where, int pagina, int qntItens, long minCap = 0, long maxCap = long.MaxValue, Func<Investimento, decimal> ordenador = default, bool desc = false, bool isEntrada = true)
        {
            List<InvestimentoGenerico> investimentos = new();

            do
            {
                List<InvestimentoGenerico> dbInvestimentos = new();

                if (desc == true)
                {
                    dbInvestimentos = _ctx.Investimentos
                    .Where(where)
                    .Where(o => o.IsEntrada == isEntrada)
                    .Include(I => I.IdInvestimentoOptionNavigation.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation)
                    .OrderByDescending(ordenador)
                    .AsEnumerable()
                    .Where(
                        i => i.IdInvestimentoOptionNavigation.ValorAcao * i.IdInvestimentoOptionNavigation.QntCotasTotais >= minCap &&
                        i.IdInvestimentoOptionNavigation.ValorAcao* i.IdInvestimentoOptionNavigation.QntCotasTotais <= maxCap)
                    .GroupBy(i => i.IdInvestimentoOption)
                    .Select(group => new List<Investimento>(group))
                    .Select(group => new InvestimentoGenerico(group))
                    .Skip((pagina - 1) * qntItens)
                    .Take(qntItens)
                    .ToList();
                } else
                {
                    dbInvestimentos = _ctx.Investimentos
                    .Where(where)
                    .Where(i => i.IsEntrada == isEntrada)
                    .Include(I => I.IdInvestimentoOptionNavigation.IdAreaInvestimentoNavigation.IdTipoInvestimentoNavigation)
                    .OrderBy(ordenador)
                    .AsEnumerable()
                    .Where(
                        i => i.IdInvestimentoOptionNavigation.ValorAcao * i.IdInvestimentoOptionNavigation.QntCotasTotais >= minCap &&
                        i.IdInvestimentoOptionNavigation.ValorAcao * i.IdInvestimentoOptionNavigation.QntCotasTotais <= maxCap)
                    .GroupBy(i => i.IdInvestimentoOption)
                    .Select(group => new List<Investimento>(group))
                    .Select(group => new InvestimentoGenerico(group))
                    .Skip((pagina - 1) * qntItens)
                    .Take(qntItens)
                    .ToList();
                }

                if (dbInvestimentos.Count > 0)
                {
                    investimentos.AddRange(dbInvestimentos);
                } 
                else
                {
                    return investimentos;
                }

                pagina += 1;
            } while (investimentos.Count != qntItens);

            return investimentos;
        }

        public int CountWhere(Expression<Func<Investimento, bool>> where)
        {
            return _ctx.Investimentos
                .Where(where)
                .GroupBy(i => i.IdInvestimentoOption)
                .Count();
        }

        public List<InvestimentoGenerico> GetCarteira(Expression<Func<Investimento, bool>> predicado, int pagina, int qntItens, long minCap = 0, long maxCap = long.MaxValue, Func<Investimento, decimal> ordenador = default, bool desc = false)
        {

            List<InvestimentoGenerico> depositos = AllWhere(predicado, pagina, qntItens, minCap, maxCap, ordenador, desc, true);
            List<InvestimentoGenerico> saques = AllWhere(predicado, pagina, qntItens, minCap, maxCap, ordenador, desc, false);

            List<InvestimentoGenerico> carteiraList = new();

            if (depositos.Count == 0)
            {
                return carteiraList;
            }

            foreach (InvestimentoGenerico deposito in depositos)
            {
                List<InvestimentoGenerico> saquesInvestimento = saques.Where(s => s.IdInvestimentoOption == deposito.IdInvestimentoOption).ToList();

                if (saquesInvestimento.Count == 1)
                {
                    InvestimentoGenerico saque = saquesInvestimento.First();

                    carteiraList.Add(new InvestimentoGenerico
                    {
                        IdUsuario = deposito.IdUsuario,
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
                        IdUsuario = deposito.IdUsuario,
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

        public InvestimentoGenerico GetCarteiraItem(Expression<Func<Investimento, bool>> predicado)
        {
            List<InvestimentoGenerico> depositos = AllWhere(predicado, 1, 1, 0, long.MaxValue, o => o.IdInvestimento, true, true);
            List<InvestimentoGenerico> saques = AllWhere(predicado, 1, 1, 0, long.MaxValue, o => o.IdInvestimento, true, false);
            
            if (depositos.Count == 0)
            {
                return null;
            }

            InvestimentoGenerico deposito = depositos.First();
            InvestimentoGenerico saque = saques.FirstOrDefault();

            if (saque == null)
            {
                return deposito;
            }
            
            if (deposito.QntCotas <= saque.QntCotas)
            {
                return null;
            }

            return new InvestimentoGenerico
            {
                IdInvestimento = deposito.IdInvestimentoOption,
                IdUsuario = deposito.IdUsuario,
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
