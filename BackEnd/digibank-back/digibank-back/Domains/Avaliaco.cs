using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Avaliaco
    {
        public short IdAvaliacao { get; set; }
        public byte? IdProduto { get; set; }
        public decimal Nota { get; set; }
        public string Comentario { get; set; }

        public virtual Produto IdProdutoNavigation { get; set; }
    }
}
