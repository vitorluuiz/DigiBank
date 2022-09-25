using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface ITipoFundoRepository
    {
        void Cadastrar(Fundo newFundo);
        void Deletar(int idFundo);
        List<Fundo> ListarTodos();
    }
}
