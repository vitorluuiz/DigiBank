using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class TipoInvestimento
    {
        public TipoInvestimento()
        {
            InvestimentoOptions = new HashSet<InvestimentoOption>();
        }

        public byte IdTipoInvestimento { get; set; }
        public string TipoInvestimento1 { get; set; }

        public virtual ICollection<InvestimentoOption> InvestimentoOptions { get; set; }
    }
}
