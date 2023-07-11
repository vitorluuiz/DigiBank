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
                .Where(I => I.IdUsuario == idUsuario)
                .Include(I => I.IdInvestimentoOptionNavigation)
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
            decimal RetornaInvestimentos(int idUsuario, int idTipoInvestimento) //Não funciona corretamente
            {

                if (idTipoInvestimento != 0)
                {
                    return ctx.Investimentos
                        .Where(I => I.IdUsuario == idUsuario && I.IdInvestimento == idTipoInvestimento)
                        .Include(I => I.IdInvestimentoOptionNavigation)
                        .Sum(I => I.QntCotas * I.IdInvestimentoOptionNavigation.ValorAcao);
                }

                return ctx.Investimentos
                    .Where(I => I.IdUsuario == idUsuario)
                    .Include(I => I.IdInvestimentoOptionNavigation)
                    .Sum(I => I.QntCotas * I.IdInvestimentoOptionNavigation.ValorAcao);
            }

            decimal RetornaRendaFixa(int idUsuario, int idTipoInvestimento) // Aqui ainda não são calculados ganhos com possíveis Juros
            {
                List<Investimento> investido =  ctx.Investimentos
                    .Where(I => I.IdUsuario == idUsuario && I.IdInvestimentoOptionNavigation.IdTipoInvestimento == idTipoInvestimento)
                    .ToList();

                decimal juros = (decimal)ctx.InvestimentoOptions.FirstOrDefault(O => O.IdInvestimentoOption == idTipoInvestimento).PercentualDividendos;

                return (decimal)investido.Sum(D => D.DepositoInicial + //Soma do produto de Valor depositado
                    D.DepositoInicial * D.IdInvestimentoOptionNavigation.PercentualDividendos / 100 * //Percentual da poupanca (Sem verificar alteracoes)
                    Convert.ToDecimal((DateTime.Now - D.DataAquisicao).TotalDays / 30)); //Total de meses
            }

            return new ExtratoInvestimentos
            {
                IdUsuario = idUsuario,
                Horario = DateTime.Now,
                Total = RetornaInvestimentos(idUsuario, 0),
                Poupanca = RetornaRendaFixa(idUsuario, 1),
                RendaFixa = RetornaRendaFixa(idUsuario, 2),
                Acoes = RetornaInvestimentos(idUsuario, 3),
                Fundos = RetornaInvestimentos(idUsuario, 4),
                Criptomoedas = RetornaInvestimentos(idUsuario, 5)
            };
        }
    }
}
