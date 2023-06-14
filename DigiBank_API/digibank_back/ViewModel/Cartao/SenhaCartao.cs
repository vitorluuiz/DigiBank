using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Cartao
{
    public class SenhaCartao
    {
        [StringLength(4, ErrorMessage = "A senha deve conter exatamente 4 Dígitos")]
        public string newToken { get; set; }
    }
}
