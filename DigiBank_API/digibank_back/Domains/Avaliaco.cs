using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Avaliaco
    {
        public short IdAvaliacao { get; set; }
        public short? IdUsuario { get; set; }
        public byte? IdPost { get; set; }
        public decimal Nota { get; set; }
        public string Comentario { get; set; }

        public virtual Marketplace IdPostNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
