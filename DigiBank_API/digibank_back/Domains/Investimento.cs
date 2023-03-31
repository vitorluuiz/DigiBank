using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Investimento
    {
        public short IdInvestimento { get; set; }
        public short IdUsuario { get; set; }
        public short IdInvestimentoOption { get; set; }
        public decimal DepositoInicial { get; set; }
        public decimal QntCotas { get; set; }
        public DateTime DataAquisicao { get; set; }

        public virtual InvestimentoOption IdInvestimentoOptionNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
