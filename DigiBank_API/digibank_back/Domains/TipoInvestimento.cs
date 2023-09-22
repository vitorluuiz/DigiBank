using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class TipoInvestimento
    {
        public TipoInvestimento()
        {
            AreaInvestimentos = new HashSet<AreaInvestimento>();
        }

        public byte IdTipoInvestimento { get; set; }
        public string TipoInvestimento1 { get; set; }

        public virtual ICollection<AreaInvestimento> AreaInvestimentos { get; set; }
    }
}
