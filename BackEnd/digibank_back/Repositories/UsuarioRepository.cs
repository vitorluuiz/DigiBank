using digibank_back.Domains;
using digibank_back.Interfaces;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        public void AdicionarDigiPoints(int idUSuario, int qntDigiPoints)
        {
            throw new System.NotImplementedException();
        }

        public void AdicionarSaldo(float valor)
        {
            throw new System.NotImplementedException();
        }

        public void AlterarApelido(int idUsuario, Usuario newApelido)
        {
            throw new System.NotImplementedException();
        }

        public void AlterarRendaFixa(int idUsuario, float newRenda)
        {
            throw new System.NotImplementedException();
        }

        public void AlterarSenha(int idUsuario, Usuario newSenha)
        {
            throw new System.NotImplementedException();
        }

        public void Atualizar(int idUsuario, Usuario usuarioAtualizado)
        {
            throw new System.NotImplementedException();
        }

        public void Cadastrar(Usuario newUsuario)
        {
            throw new System.NotImplementedException();
        }

        public bool Deletar(int idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public Usuario ListarPorCpf(string Cpf)
        {
            throw new System.NotImplementedException();
        }

        public Usuario ListarPorId(int idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public List<Usuario> ListarTodos()
        {
            throw new System.NotImplementedException();
        }

        public Usuario Login(string cpf, string senha)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoverDigiPoints(int idUsuario, int qntDigiPoints)
        {
            throw new System.NotImplementedException();
        }

        public bool RemoverSaldo(float valor)
        {
            throw new System.NotImplementedException();
        }

        public bool VerificarDisponibilidade(Usuario newUsuario)
        {
            throw new System.NotImplementedException();
        }
    }
}
