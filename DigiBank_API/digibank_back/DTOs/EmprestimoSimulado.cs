using digibank_back.Domains;
using digibank_back.Utils;
using System;

namespace digibank_back.DTOs
{
    public class EmprestimoSimulado
    {
        public decimal ParceladoInicial { get; set; }
        public decimal RestanteParcelado { get; set; }
        public decimal ParcelaSugerida { get; set; } 
        public double ProgressoPagamento { get; set; }
        public double ProgressoPrazo { get; set; }
        public decimal RestanteAvista { get; set; }
        public DateTime ProximaParcela { get; set; }

        public EmprestimoSimulado(EmprestimosOption o)
        {
            RestanteParcelado = EmprestimoCalc.CalcularParcelado(o.Valor, (decimal)o.TaxaJuros, DateTime.Now.AddDays(o.PrazoEstimado) - DateTime.Now, 0);
            ParcelaSugerida = EmprestimoCalc.SugerirParcela(o.Valor, (decimal)o.TaxaJuros, DateTime.Now.AddDays(o.PrazoEstimado) - DateTime.Now);
            ProgressoPagamento = 0;
            ProgressoPrazo = 0;
            RestanteAvista = o.Valor;
            ProximaParcela = DateTime.Now.AddDays(30);
        }

        public EmprestimoSimulado(Emprestimo e)
        {
            double taxaJuros;
            if (e.IdCondicao == 3)
            {
                taxaJuros = (double)e.IdEmprestimoOptionsNavigation.TaxaJuros * 1.2;
            } else
            {
                taxaJuros = (double)e.IdEmprestimoOptionsNavigation.TaxaJuros;
            }

            ProgressoPrazo = EmprestimoCalc.RazaoDatas(DateTime.Now - e.DataInicial, e.DataFinal - e.DataInicial);
            ProximaParcela = EmprestimoCalc.ProxPagamento(e.UltimoPagamento, e.DataFinal);
            RestanteAvista = EmprestimoCalc.CalcularAvista(
                e.IdEmprestimoOptionsNavigation.Valor,
                taxaJuros,
                e.DataFinal - e.DataInicial,
                ProgressoPrazo,
                e.ValorPago
                );
            RestanteParcelado = EmprestimoCalc.CalcularParcelado(
                e.IdEmprestimoOptionsNavigation.Valor,
                (decimal)taxaJuros,
                e.DataFinal - e.DataInicial,
                e.ValorPago
                );
            ParceladoInicial = EmprestimoCalc.CalcularParcelado(
                e.IdEmprestimoOptionsNavigation.Valor,
                (decimal)taxaJuros,
                e.DataFinal - e.DataInicial,
                0
                );
            ParcelaSugerida = EmprestimoCalc.SugerirParcela(RestanteParcelado, (decimal)taxaJuros, e.DataFinal - e.UltimoPagamento);
            ProgressoPagamento = EmprestimoCalc.RazaoPagamento(e.ValorPago, ParceladoInicial);
        }
    }
}
