using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Meta
    {
        public short IdMeta { get; set; }
        public int? IdUsuario { get; set; }
        public string Titulo { get; set; }
        public decimal ValorMeta { get; set; }
        public decimal? Arrecadado { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
