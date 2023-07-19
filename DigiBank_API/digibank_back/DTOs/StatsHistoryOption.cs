namespace digibank_back.DTOs
{
    public class StatsHistoryOption
    {
        public int IdInvestimentoOption { get; set; }
        public int IdTipoInvestimento { get; set; }
        public decimal Max { get; set; }
        public decimal Min { get; set; }
        public decimal VariacaoMinMax { get; set; }
        public decimal VariacaoMinMaxPorcentual { get; set; }
        public decimal VariacaoPeriodo { get; set; }
        public decimal VariacaoPeriodoPercentual { get; set; }
        public decimal Media { get; set; }
        public decimal CoeficienteVariativo { get; set; }
    }
}
