using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Inventario
    {
        public short IdInventario { get; set; }
        public int IdUsuario { get; set; }
        public byte IdPost { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataAquisicao { get; set; }

        public virtual Marketplace IdPostNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
