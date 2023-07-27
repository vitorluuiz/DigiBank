namespace digibank_back.DTOs
{
    public class StatsHistoryOption
    {
        public int IdInvestimentoOption { get; set; }
        public decimal Valor { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Dividendos { get; set; }
        public decimal Max { get; set; }
        public decimal Min { get; set; }
        public decimal MinMax { get; set; }
        public decimal MinMaxPercentual { get; set; }
        public decimal VariacaoPeriodo { get; set; }
        public decimal VariacaoPeriodoPercentual { get; set; }
        public decimal Media { get; set; }
        public decimal CoeficienteVariativo { get; set; }
    }
}
