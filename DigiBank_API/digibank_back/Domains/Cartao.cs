using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Cartao
    {
        public short IdCartao { get; set; }

        [Required(ErrorMessage = "idUsuario é obrigatório")]
        public short IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Numero { get; set; }

        [Required(ErrorMessage = "Token é obrigatório")]
        public string Token { get; set; }
        public string Cvv { get; set; }
        public bool IsValid { get; set; }
        public DateTime DataExpira { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
