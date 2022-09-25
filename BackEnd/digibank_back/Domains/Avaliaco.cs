using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Avaliaco
    {
        public short IdAvaliacao { get; set; }
        public byte? IdProduto { get; set; }

        [Required(ErrorMessage = "A nota é obrigatória")]
        public decimal Nota { get; set; }
        public string Comentario { get; set; }

        public virtual Produto IdProdutoNavigation { get; set; }
    }
}
