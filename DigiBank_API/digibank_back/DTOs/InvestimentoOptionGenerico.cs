using System;

namespace digibank_back.DTOs
{
    public class InvestimentoOptionGenerico
    {
        public short IdInvestimentoOption { get; set; }
        public byte IdTipoInvestimento { get; set; }
        public short IdAreaInvestimento { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }
        public int Colaboradores { get; set; }
        public decimal ValorAcao { get; set; }
        public int QntCotasTotais { get; set; }
        public DateTime Fundacao { get; set; }
        public DateTime Abertura { get; set; }
        public string Sede { get; set; }
        public string Fundador { get; set; }
        public decimal? PercentualDividendos { get; set; }
        public DateTime Tick { get; set; }
    }
}
