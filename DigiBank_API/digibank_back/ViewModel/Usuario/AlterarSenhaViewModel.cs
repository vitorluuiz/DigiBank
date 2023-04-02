using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Usuario
{
    public class AlterarSenhaViewModel
    {
        [Required(ErrorMessage = "Informe o idUsuario")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Informe a senha atual")]
        public string senhaAtual { get; set; }

        [Required(ErrorMessage = "Informe a nova senha")]
        public string newSenha { get; set; }
    }
}
