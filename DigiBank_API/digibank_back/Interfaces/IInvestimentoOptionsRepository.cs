using digibank_back.Domains;
using digibank_back.DTOs;
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
        List<EmblemaInvestOption> ListarEmblemas(int idInvestimentoOption, int days);
        List<double> ListarIndices(int idInvestimentoOption, int days);
        StatsHistoryOption ListarStatsHistoryOption(int idOption, int days);
    }
}
