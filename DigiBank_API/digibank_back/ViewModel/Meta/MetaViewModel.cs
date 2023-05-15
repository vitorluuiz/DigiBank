using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Meta
{
    public class MetaViewModel
    {
        [Required(ErrorMessage = "IdUsuario é obrigatório")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Titulo é obrigatório")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "ValorMeta é obrigatório")]
        public decimal ValorMeta { get; set; }
    }
}
