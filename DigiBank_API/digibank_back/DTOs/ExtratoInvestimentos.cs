using System;

namespace digibank_back.DTOs
{
    public class ExtratoInvestimentos
    {
        public DateTime Horario { get; set; }
        public int IdUsuario { get; set; }
        public decimal Total { get; set; }
        public decimal RendaFixa { get; set; }
        public decimal Acoes { get; set; }
        public decimal Criptomoedas { get; set; }
        public decimal Fundos { get; set; }
        public decimal Poupanca { get; set; }
    }
}
