using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using digibank_back.Contexts;
using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class InvestimentoGenerico
    {
        public short IdInvestimento { get; set; }
        public int IdUsuario { get; set; }
        public short IdInvestimentoOption { get; set; }
        public decimal QntCotas { get; set; }
        public decimal DepositoInicial { get; set; }
        public bool IsEntrada { get; set; }
        public DateTime DataAquisicao { get; set; }
        public InvestimentoOptionGenerico IdInvestimentoOptionNavigation { get; set; }

        public InvestimentoGenerico()
        {

        }

        public InvestimentoGenerico(List<Investimento> investimentos)
        {
            Investimento i = investimentos.First();

            IdInvestimento = i.IdInvestimento;
            IdInvestimentoOption = i.IdInvestimentoOption;
            IdUsuario = i.IdUsuario;
            DepositoInicial = investimentos.Sum(i => i.DepositoInicial);
            QntCotas = investimentos.Sum(i => i.QntCotas);
            DataAquisicao = i.DataAquisicao;
            IsEntrada = i.IsEntrada;
            IdInvestimentoOptionNavigation = new InvestimentoOptionGenerico(i.IdInvestimentoOptionNavigation);
        }
    }
}