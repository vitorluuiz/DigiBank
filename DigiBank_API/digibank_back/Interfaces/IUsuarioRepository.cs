using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IUsuarioRepository
    {
        bool Cadastrar(Usuario newUsuario);
        bool Atualizar(int idUsuario, Usuario usuarioAtualizado);
        bool VerificarDisponibilidade(Usuario newUsuario);
        bool Deletar(int idUsuario);
        List<Usuario> ListarTodos();
        Usuario Login(string cpf, string senha);
        Usuario ListarPorId(int idUsuario);
        PublicUsuario ListarPorCpf(string Cpf);
        void AlterarApelido(int idUsuario, string newApelido);
        bool AlterarSenha(int idUsuario, string senhaAtual, string newSenha);
        void AlterarRendaFixa(int idUsuario, decimal newRenda);
        void AdicionarSaldo(int idUsuario, decimal valor);
        bool RemoverSaldo(short idUsuario, decimal valor);
        void AdicionarDigiPoints(int idUsuario, decimal qntDigiPoints);
        bool RemoverDigiPoints(int idUsuario, decimal qntDigiPoints);
    }
}
