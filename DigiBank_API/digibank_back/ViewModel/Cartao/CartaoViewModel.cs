using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Cartao
{
    public class CartaoViewModel
    {
        public int IdCartao { get; set; }

        [Required(ErrorMessage = "Token é obrigatório")]
        public string Token { get; set; }

        [Required(ErrorMessage = "Número é obrigatório")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Cvv é obrigatório")]
        public string Cvv { get; set; }
    }
}
