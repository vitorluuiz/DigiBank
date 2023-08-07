
using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class InvestimentoTitle
    {

        public int IdInvestimentoOption { get; set; }
        public string Nome { get; set; }
        public decimal Valor { get; set; }
        public decimal VariacaoPercentual { get; set; }
        public string Logo { get; set; }

        public InvestimentoTitle(InvestimentoOption option)
        {
            IdInvestimentoOption = option.IdInvestimentoOption;
            Nome = option.Nome;
            Valor = option.ValorAcao;
            Logo = option.Logo;
        }
    }
}
