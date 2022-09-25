using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Produto
    {
        public Produto()
        {
            Avaliacos = new HashSet<Avaliaco>();
        }

        public byte IdProduto { get; set; }
        public short? IdUsuario { get; set; }
        public decimal Valor { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string ProdutoImg { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual ICollection<Avaliaco> Avaliacos { get; set; }
    }
}
