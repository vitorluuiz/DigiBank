using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class InvestimentoOption
    {
        public InvestimentoOption()
        {
            Investimentos = new HashSet<Investimento>();
        }

        public short IdInvestimentoOption { get; set; }
        public byte IdTipoInvestimento { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string CodeId { get; set; }
        public string Img { get; set; }
        public decimal ValorInicial { get; set; }
        public decimal IndiceConfiabilidade { get; set; }
        public decimal IndiceDividendos { get; set; }
        public decimal IndiceValorizacao { get; set; }
        public decimal Dividendos { get; set; }

        public virtual TipoInvestimento IdTipoInvestimentoNavigation { get; set; }
        public virtual ICollection<Investimento> Investimentos { get; set; }
    }
}
