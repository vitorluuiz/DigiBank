using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class AcoesOption
    {
        public AcoesOption()
        {
            Acos = new HashSet<Aco>();
        }

        public byte IdAcaoOption { get; set; }
        public byte? IdTipoAcao { get; set; }
        public decimal IndiceConfiabilidade { get; set; }
        public decimal IndiceDividendos { get; set; }
        public decimal IndiceValorizacao { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Codigo { get; set; }
        public decimal DividendoAnual { get; set; }
        public short CotasDisponiveis { get; set; }
        public string AcaoImg { get; set; }

        public virtual TiposAco IdTipoAcaoNavigation { get; set; }
        public virtual ICollection<Aco> Acos { get; set; }
    }
}
