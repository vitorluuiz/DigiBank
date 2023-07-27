using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class TipoPercentil
    {
        public TipoPercentil()
        {
            PercentilTables = new HashSet<PercentilTable>();
        }

        public byte IdTipoPercentil { get; set; }
        public string Tipo { get; set; }

        public virtual ICollection<PercentilTable> PercentilTables { get; set; }
    }
}
