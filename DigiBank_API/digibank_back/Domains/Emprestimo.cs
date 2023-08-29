using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class Emprestimo
    {
        public short IdEmprestimo { get; set; }
        public int IdUsuario { get; set; }
        public byte IdCondicao { get; set; }
        public byte IdEmprestimoOptions { get; set; }
        public decimal ValorPago { get; set; }
        public DateTime UltimoPagamento { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }

        public virtual Condico IdCondicaoNavigation { get; set; }
        public virtual EmprestimosOption IdEmprestimoOptionsNavigation { get; set; }
        public virtual Usuario IdUsuarioNavigation { get; set; }
    }
}
