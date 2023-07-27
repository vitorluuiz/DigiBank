using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class PercentilTable
    {
        public short IdPercentil { get; set; }
        public short IdInvestimentoOption { get; set; }
        public byte IdTipoPercentil { get; set; }
        public decimal MarketCap { get; set; }
        public decimal Dividendos { get; set; }
        public decimal ValorizacaoPercentual { get; set; }
        public decimal Confiabilidade { get; set; }

        public virtual InvestimentoOption IdInvestimentoOptionNavigation { get; set; }
        public virtual TipoPercentil IdTipoPercentilNavigation { get; set; }
    }
}
