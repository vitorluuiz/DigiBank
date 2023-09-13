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

        [StringLength(11, ErrorMessage = "O CPF precisa ter 11 caracteres")]
        [Required(ErrorMessage = "CPF obrigatório")]
        public string Cpf { get; set; }

        [StringLength(11, ErrorMessage = "Telefone precisa ter 11 caracteres")]
        [Required(ErrorMessage = "Telefone obrigatório")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Email obrigatório")]
        [MaxLength(255, ErrorMessage = "Email deve ter até 255 caracteres")]
        public string Email { get; set; }

        [MinLength(8, ErrorMessage = "A senha precisa ter ao menos 8 dígitos")]
        [MaxLength(32, ErrorMessage = "O limite da senha é de 32 caracteres")]
        [Required(ErrorMessage = "Senha obrigatória")]
        public string Senha { get; set; }
        public decimal RendaFixa { get; set; }
    }
}
