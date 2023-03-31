using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface ITipoInvestimentoRepository
    {
        void Cadastrar(TipoInvestimento newTipoInvestimento);
        void Deletar(int idTipoInvestimento);
        List<TipoInvestimento> ListarTodos();
    }
}
