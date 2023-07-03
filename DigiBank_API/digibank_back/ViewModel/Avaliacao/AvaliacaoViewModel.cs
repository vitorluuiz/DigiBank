using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Avaliacao
{
    public class AvaliacaoViewModel
    {
        public int IdAvaliacao { get; set; }

        [Required(ErrorMessage = "IdPost é obrigatório")]
        public int IdPost { get; set; }

        [Required(ErrorMessage = "IdUsuário é obrigatório")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Nota da avaliação é obrigatório")]
        public int Nota { get; set; }
        public string Comentario { get; set; }
    }
}
