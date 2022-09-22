using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IUsuarioRepository
    {
        void Cadastrar(Usuario newUsuario);
        void Atualizar(int idUsuario, Usuario usuarioAtualizado);
        bool VerificarDisponibilidade(Usuario newUsuario);
        bool Deletar(int idUsuario);
        List<Usuario> ListarTodos();
        Usuario Login(string cpf, string senha);
        Usuario ListarPorId(int idUsuario);
        Usuario ListarPorCpf(string Cpf);
        void AlterarApelido(int idUsuario, Usuario newApelido);
        void AlterarSenha(int idUsuario, Usuario newSenha);
        void AlterarRendaFixa(int idUsuario, float newRenda);
        void AdicionarSaldo(float valor);
        bool RemoverSaldo(float valor);
        void AdicionarDigiPoints(int idUSuario, int qntDigiPoints);
        bool RemoverDigiPoints(int idUsuario, int qntDigiPoints);
    }
}
