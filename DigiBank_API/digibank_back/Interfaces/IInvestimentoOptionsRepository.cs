using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace digibank_back.Repositories
{
    public interface IInvestimentoOptionsRepository
    {
        InvestimentoOption CreateFicOption();
        void Update(short id, InvestimentoOption updatedO);
        List<InvestimentoOptionMinimo> AllWhere(Expression<Func<InvestimentoOption, bool>> predicado, int pagina, int qntitens);
        int CountWhere(Expression<Func<InvestimentoOption, bool>> predicado);
        List<InvestimentoOptionMinimo> ListarCompradosAnteriormente(int pagina, int qntItens, byte idTipoInvestimentoOption, int idUsuario);
        List<InvestimentoOptionMinimo> ListarTodosPorId(int[] ids, byte idOption);
        List<InvestimentoTitle> BuscarInvestimentos(byte idOption, int qntItens);
        InvestimentoOptionGenerico ListarPorId(int idOption);
        List<EmblemaInvestOption> ListarEmblemas(int idOption, int days);
        List<double> Indices(int idOption, int days);
    }
}
