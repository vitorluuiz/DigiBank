using System;
using System.Collections.Generic;

#nullable disable

namespace digibank_back.Domains
{
    public partial class InvestimentoOption
    {
        public InvestimentoOption()
        {
            HistoricoInvestimentoOptions = new HashSet<HistoricoInvestimentoOption>();
            Investimentos = new HashSet<Investimento>();
        }

        public short IdInvestimentoOption { get; set; }
        public byte IdTipoInvestimento { get; set; }
        public short IdAreaInvestimento { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; } 
        public int Colaboradores { get; set; }
        public decimal ValorAcao { get; set; }
        public short QntCotasTotais { get; set; }
        public DateTime Fundacao { get; set; }
        public DateTime Abertura { get; set; }
        public string Sede { get; set; }
        public string Fundador { get; set; }
        public decimal? PercentualDividendos { get; set; }
        public DateTime Tick { get; set; }

        public virtual AreaInvestimento IdAreaInvestimentoNavigation { get; set; }
        public virtual TipoInvestimento IdTipoInvestimentoNavigation { get; set; }
        public virtual ICollection<HistoricoInvestimentoOption> HistoricoInvestimentoOptions { get; set; }
        public virtual ICollection<Investimento> Investimentos { get; set; }
    }
}
