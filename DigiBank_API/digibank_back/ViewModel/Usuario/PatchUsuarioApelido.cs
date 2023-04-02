using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Usuario
{
    public class PatchUsuarioApelido
    {
        [Required(ErrorMessage = "Informe o idUsuario")]
        public int idUsuario { get; set; }

        [Required(ErrorMessage = "Informe o novo apelido")]
        public string newApelido { get; set; }
    }
}
