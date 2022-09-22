using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface ICondicaoRepository
    {
        void Cadastrar(Condico newCondicao);
        void Deletar(int idCondicao);
        List<Condico> ListarTodas();
    }
}
