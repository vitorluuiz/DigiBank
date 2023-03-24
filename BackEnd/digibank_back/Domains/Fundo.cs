using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Fundo
    {
        public short IdFundo { get; set; }
        public short? IdUsuario { get; set; }
        public short? IdfundosOptions { get; set; }
        public decimal DepositoInicial { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFinal { get; set; }

        public virtual Usuario IdUsuarioNavigation { get; set; }
        public virtual FundosOption IdfundosOptionsNavigation { get; set; }
    }
}
