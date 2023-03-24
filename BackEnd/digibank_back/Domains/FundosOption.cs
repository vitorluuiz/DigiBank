using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class FundosOption
    {
        public FundosOption()
        {
            Fundos = new HashSet<Fundo>();
        }

        public short IdFundosOption { get; set; }
        public byte? IdTipoFundo { get; set; }
        public decimal IndiceConfiabilidade { get; set; }
        public decimal IndiceDividendos { get; set; }
        public decimal IndiceValorizacao { get; set; }
        public decimal TaxaJuros { get; set; }

        public virtual TiposFundo IdTipoFundoNavigation { get; set; }
        public virtual ICollection<Fundo> Fundos { get; set; }
    }
}
