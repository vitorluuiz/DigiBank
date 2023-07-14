using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IInvestimentoOptionsRepository
    {
        InvestimentoOption CreateFicOption();
        void Atualizar(int idInvestimentoOption, InvestimentoOption optionAtualizada);
        List<InvestimentoOption> ListarTodos(int pagina, int qntItens);
        List<InvestimentoOption> ListarTodosPorId(int[] ids);
        InvestimentoOption ListarPorId(int idInvestimentoOption);
        string[] ListarEmblemas(int idInvestimentoOption);
        int[] ListarIndices(int idInvestimentoOption);
    }
}
