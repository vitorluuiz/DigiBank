using digibank_back.Domains;

namespace digibank_back.DTOs
{
    public class UsuarioInfos
    {
        public int IdUsuario { get; set; }
        public string NomeCompleto { get; set; }
        public string Apelido { get; set; }
        public decimal Saldo { get; set; }
        public decimal DigiPoints { get; set; }
        public decimal Investido { get; set; }
        public Meta MetaDestaque { get; set; }

        public UsuarioInfos(Usuario u)
        {
            IdUsuario = u.IdUsuario;
            NomeCompleto = u.NomeCompleto;
            Apelido = u.Apelido;
            Saldo = u.Saldo;
            DigiPoints = u.DigiPoints;
        }
    }
}
