using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Meta
{
    public class AmountMetaViewModel
    {
        [Required(ErrorMessage = "Amount é um parâmetro obrigatório")]
        public decimal Amount { get; set; }
    }
}
