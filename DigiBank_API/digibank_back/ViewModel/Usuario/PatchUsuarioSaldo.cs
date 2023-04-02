using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Usuario
{
    public class PatchUsuarioSaldo
    {
        [Required(ErrorMessage = "Informe o idUsuario")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Informe o valor")]
        public decimal valor { get; set; }
    }
}
