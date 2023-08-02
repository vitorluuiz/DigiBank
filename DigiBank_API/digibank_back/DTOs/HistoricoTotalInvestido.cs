using System;

namespace digibank_back.DTOs
{
    public class HistoricoTotalInvestido
    {
        public int IdHistorico { get; set; }
        public DateTime DataH { get; set; }
        public decimal Valor { get; set; }
    }
}
