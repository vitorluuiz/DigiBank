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
        public int IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsActive { get; set; }
        public short? Vendas { get; set; }
        public short? QntAvaliacoes { get; set; }
        public decimal? Avaliacao { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Avaliaco> Avaliacos { get; set; }
        public virtual ICollection<ImgsPost> ImgsPosts { get; set; }
        public virtual ICollection<Inventario> Inventarios { get; set; }
    }
}
