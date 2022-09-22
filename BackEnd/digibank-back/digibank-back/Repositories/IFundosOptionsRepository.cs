using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IFundosOptionsRepository
    {
        void Cadastrar(FundosOption newFundoOption);
        void Atualizar(int idFundoOption, FundosOption fundoAtualizado);
        void Deletar(int idFundoOption);
        List<FundosOption> ListarTodos();
        FundosOption ListarPorId(int idFundoOption);
    }
}
