using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IInvestimentoOptionsRepository
    {
        InvestimentoOption CreateFicOption();
        void Atualizar(short idInvestimentoOption, InvestimentoOption optionAtualizada);
        void Deletar(short idInvestimentoOption);
        List<InvestimentoOptionGenerico> ListarTodos(int pagina, int qntItens);
        List<InvestimentoOptionGenerico> ListarCompradosAnteriormente(int pagina, int qntItens, byte idTipoInvestimentoOption, int idUsuario);
        List<InvestimentoOption> ListarTodosPorId(int[] ids);;
        List<InvestimentoTitle> BuscarInvestimentos(byte idTipoInvestimentoOption, int qntItens);
        List<InvestimentoOptionGenerico> ListarPorTipoInvestimento(byte idTipoInvestimentoOption, int pagina, int qntItens);
        InvestimentoOption ListarPorId(int idInvestimentoOption);
        List<EmblemaInvestOption> ListarEmblemas(int idInvestimentoOption, int days);
        List<double> ListarIndices(int idInvestimentoOption, int days);
        StatsHistoryOption ListarStatsHistoryOption(int idOption, int days);
    }
}
