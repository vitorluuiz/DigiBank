using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class AreaInvestimento
    {
        public AreaInvestimento()
        {
            InvestimentoOptions = new HashSet<InvestimentoOption>();
        }

        public short IdAreaInvestimento { get; set; }
        public string Area { get; set; }

        public virtual ICollection<InvestimentoOption> InvestimentoOptions { get; set; }
    }
}
