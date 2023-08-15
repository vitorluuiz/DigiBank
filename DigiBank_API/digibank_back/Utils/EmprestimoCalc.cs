using System;

namespace digibank_back.Utils
{
    public class EmprestimoCalc
    {
        public static double RazaoDatas(TimeSpan percorrido, TimeSpan total)
        {
            return percorrido.TotalDays / total.TotalDays;
        }
        
        public static DateTime ProxPagamento(DateTime ultimoPagamento, DateTime prazo)
        {
            if (ultimoPagamento.AddDays(30) >= prazo)
            {
                return prazo;
            }
            else
            {
                return ultimoPagamento.AddDays(30);
            }
        }

        public static decimal CalcularAvista(decimal valor, double jurosMensal, TimeSpan periodo, double progressoPrazo, decimal valorPago)
        {
            double jurosTotal = jurosMensal * periodo.TotalDays / 30;
            decimal jurosAplicados = valor * (decimal)jurosTotal * (decimal)progressoPrazo / 100;
            return valor + jurosAplicados - valorPago;
        }

        public static decimal CalcularParcelado(decimal valor, decimal jurosMensal, TimeSpan periodo, decimal valorPago)
        {
            decimal jurosAplicados = valor * jurosMensal * (decimal)periodo.TotalDays / 30 / 100;
            return valor + jurosAplicados - valorPago;
        }

        public static decimal SugerirParcela(decimal valorRestante, decimal juros, TimeSpan periodoRestante)
        {
            int parcelas = (int)Math.Abs(periodoRestante.TotalDays / 30);
            if (parcelas != 0)
            {
                return (valorRestante / parcelas) + (valorRestante / parcelas * (juros / 100));
            }
            return valorRestante + valorRestante * juros / 100;
        }

        public static double RazaoPagamento(decimal valorPago, decimal restanteParcelado)
        {
            return (double)(valorPago / restanteParcelado);
        }
    }
}
