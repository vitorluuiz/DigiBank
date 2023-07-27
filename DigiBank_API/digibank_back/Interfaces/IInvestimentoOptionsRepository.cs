using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IInvestimentoOptionsRepository
    {
        InvestimentoOption CreateFicOption();
        void Update(short id, InvestimentoOption updatedO);
        List<InvestimentoOptionMinimo> ListarTodos(int pagina, int qntItens);
        List<InvestimentoOptionMinimo> ListarCompradosAnteriormente(int pagina, int qntItens, byte idTipoInvestimentoOption, int idUsuario);
        List<InvestimentoOptionMinimo> ListarTodosPorId(int[] ids);
        List<InvestimentoTitle> BuscarInvestimentos(byte idOption, int qntItens);
        List<InvestimentoOptionMinimo> ListarPorTipoInvestimento(byte idOption, int pagina, int qntItens);
        InvestimentoOptionGenerico ListarPorId(int idOption);
        List<EmblemaInvestOption> ListarEmblemas(int idOption, int days);
        List<double> Indices(int idOption, int days);
    }
}
