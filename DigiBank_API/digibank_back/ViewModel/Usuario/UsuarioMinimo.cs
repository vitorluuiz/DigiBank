using System.ComponentModel.DataAnnotations;

namespace digibank_back.ViewModel.Usuario
{
    public class UsuarioMinimo
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Nome completo obrigatório")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "Apelido obrigatório")]
        public string Apelido { get; set; }

        [Required(ErrorMessage = "CPF obrigatório")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Telefone obrigatório")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Email obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha obrigatória")]
        public string Senha { get; set; }
        public decimal RendaFixa { get; set; }
    }
}
