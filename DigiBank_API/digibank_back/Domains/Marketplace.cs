using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Marketplace
    {
        public Marketplace()
        {
            Avaliacos = new HashSet<Avaliaco>();
            ImgsPosts = new HashSet<ImgsPost>();
            Inventarios = new HashSet<Inventario>();
        }

        public byte IdPost { get; set; }
        public short? IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool? IsVirtual { get; set; }
        public bool? IsVisible { get; set; }
        public string MainImg { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Avaliaco> Avaliacos { get; set; }
        public virtual ICollection<ImgsPost> ImgsPosts { get; set; }
        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}
