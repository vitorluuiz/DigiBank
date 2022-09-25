using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class TiposFundo
    {
        public TiposFundo()
        {
            FundosOptions = new HashSet<FundosOption>();
        }

        public byte IdTipoFundo { get; set; }
        public string TipoFundo { get; set; }

        public virtual ICollection<FundosOption> FundosOptions { get; set; }
    }
}
