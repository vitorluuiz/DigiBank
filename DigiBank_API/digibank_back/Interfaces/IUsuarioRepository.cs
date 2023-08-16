using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IUsuarioRepository
    {
        bool Post(Usuario newUsuario);
        bool Update(int idUsuario, Usuario usuarioAtualizado);
        bool VerificarDisponibilidade(Usuario newU);
        bool Delete(int idUsuario);
        List<Usuario> Todos();
        Usuario Login(string cpf, string senha);
        UsuarioInfos Infos(int id);
        Usuario PorId(int id);
        PublicUsuario PorCpf(string Cpf);
        bool CanAddSaldo(int id, decimal valor);
        bool CanRemoveSaldo(int id, decimal valor);
        bool AdicionarSaldo(int id, decimal valor);
        bool RemoverSaldo(short id, decimal valor);
        //void AdicionarDigiPoints(int id, decimal qntDigiPoints);
        //bool RemoverDigiPoints(int id, decimal qntDigiPoints);
    }
}
