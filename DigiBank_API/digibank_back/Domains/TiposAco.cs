using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class TiposAco
    {
        public TiposAco()
        {
            AcoesOptions = new HashSet<AcoesOption>();
        }

        public byte IdTipoAcao { get; set; }
        public string TipoAcao { get; set; }

        public virtual ICollection<AcoesOption> AcoesOptions { get; set; }
    }
}
