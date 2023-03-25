using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class ImgsProduto
    {
        public short IdImg { get; set; }
        public byte? IdProduto { get; set; }
        public string Img { get; set; }

        public virtual Produto IdProdutoNavigation { get; set; }
    }
}
