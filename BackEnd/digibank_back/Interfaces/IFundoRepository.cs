using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IFundoRepository
    {
        void Comprar(Fundo newFundo);
        void Vender(int idFundo);
        Fundo ListarPorId(int idFundo);
        List<Fundo> ListarTodos();
        List<Fundo> ListarDeUsuario(int idUsuario);
    }
}
