using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Aco
    {
        public short IdAcao { get; set; }
        public short? IdUsuario { get; set; }
        public byte? IdAcoesOptions { get; set; }
        public byte QntCotas { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal ValorInicial { get; set; }

        public virtual AcoesOption IdAcoesOptionsNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
