using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class HistoricoInvestimentoOption
    {
        public int IdHistorico { get; set; }
        public short IdInvestimentoOption { get; set; }
        public DateTime DataH { get; set; }
        public decimal Valor { get; set; }

        public virtual InvestimentoOption IdInvestimentoOptionNavigation { get; set; }
    }
}
