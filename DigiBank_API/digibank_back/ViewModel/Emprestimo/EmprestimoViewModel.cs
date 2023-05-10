using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Emprestimo
{
    public class EmprestimoViewModel
    {
        [Required(ErrorMessage = "IdUsuario é obrigatório")]
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "IdEmprestimoOptions é obrigatório")]
        public int IdEmprestimoOptions { get; set; }
    }
}
