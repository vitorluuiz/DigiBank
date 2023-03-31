using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Transaco
    {
        public short IdTransacao { get; set; }
        public short IdUsuarioPagante { get; set; }
        public short IdUsuarioRecebente { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataTransacao { get; set; }
        public string Descricao { get; set; }

        public virtual Usuario IdUsuarioPaganteNavigation { get; set; }
        public virtual Usuario IdUsuarioRecebenteNavigation { get; set; }
    }
}
