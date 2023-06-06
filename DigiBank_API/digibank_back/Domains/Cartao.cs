using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Cartao
    {
        public short IdCartao { get; set; }
        public short IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Numero { get; set; }
        public string Token { get; set; }
        public string Cvv { get; set; }
        public bool IsValid { get; set; }
        public DateTime DataExpira { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
