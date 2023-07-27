using System;
using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class InvestimentoOptionMinimo
    {
        public short IdInvestimentoOption { get; set; }
        public string TipoInvestimento { get; set; }
        public string AreaInvestimento { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public string Logo { get; set; }
        public string MainImg { get; set; }
        public string MainColorHex { get; set; }
        public decimal Valor { get; set; }
        public decimal? Dividendos { get; set; }
        public decimal VariacaoPercentual { get; set; }

        public InvestimentoOptionMinimo(InvestimentoOption option)
        {

            IdInvestimentoOption = option.IdInvestimentoOption;
            TipoInvestimento = option.IdTipoInvestimentoNavigation.TipoInvestimento1;
            AreaInvestimento = option.IdAreaInvestimentoNavigation.Area;
            Nome = option.Nome;
            Sigla = option.Sigla;
            Logo = option.Logo;
            MainImg = option.MainImg;
            MainColorHex = option.MainColorHex;
            Valor = Math.Round(option.ValorAcao, 2);
            Dividendos = option.PercentualDividendos;
        }
    }
}
