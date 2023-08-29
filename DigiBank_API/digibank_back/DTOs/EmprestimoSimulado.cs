using digibank_back.Domains;
using digibank_back.Utils;
using System;

namespace digibank_back.DTOs
{
    public class EmprestimoSimulado
    {
        public decimal RestanteParcelado { get; set; }
        public decimal ParcelaSugerida { get; set; } 
        public double ProgressoPagamento { get; set; }
        public double ProgressoPrazo { get; set; }
        public decimal RestanteAvista { get; set; }
        public DateTime ProximaParcela { get; set; }

        public EmprestimoSimulado(EmprestimosOption o)
        {
            RestanteParcelado = EmprestimoCalc.CalcularParcelado(o.Valor, o.TaxaJuros, DateTime.Now.AddDays(o.PrazoEstimado) - DateTime.Now, 0);
            ParcelaSugerida = EmprestimoCalc.SugerirParcela(o.Valor, o.TaxaJuros, DateTime.Now.AddDays(o.PrazoEstimado) - DateTime.Now);
            ProgressoPagamento = 0;
            ProgressoPrazo = 0;
            RestanteAvista = o.Valor;
            ProximaParcela = DateTime.Now.AddDays(30);
        }

        public EmprestimoSimulado(Emprestimo e)
        {
            ProgressoPrazo = EmprestimoCalc.RazaoDatas(DateTime.Now - e.DataInicial, e.DataFinal - e.DataInicial);
            ProximaParcela = EmprestimoCalc.ProxPagamento(e.UltimoPagamento, e.DataFinal);
            RestanteAvista = EmprestimoCalc.CalcularAvista(
                e.IdEmprestimoOptionsNavigation.Valor,
                (double)e.IdEmprestimoOptionsNavigation.TaxaJuros,
                e.DataFinal - e.DataInicial,
                ProgressoPrazo,
                e.ValorPago
                );
            RestanteParcelado = EmprestimoCalc.CalcularParcelado(
                e.IdEmprestimoOptionsNavigation.Valor,
                e.IdEmprestimoOptionsNavigation.TaxaJuros,
                e.DataFinal - e.DataInicial,
                e.ValorPago
                );
            ParcelaSugerida = EmprestimoCalc.SugerirParcela(RestanteParcelado, e.IdEmprestimoOptionsNavigation.TaxaJuros, e.DataFinal - e.UltimoPagamento);
            ProgressoPagamento = EmprestimoCalc.RazaoPagamento(e.ValorPago, RestanteParcelado);
        }
    }
}
