using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class PublicUsuario
    {
        public int IdUsuario { get; set; }
        public string NomeCompleto { get; set; }
        public string Apelido { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }

        public PublicUsuario(Usuario u)
        {
            IdUsuario = u.IdUsuario;
            NomeCompleto = u.NomeCompleto;
            Apelido = u.Apelido;
            Cpf = u.Cpf;
            Telefone = u.Telefone;
            Email = u.Email;
        }
    }
}
