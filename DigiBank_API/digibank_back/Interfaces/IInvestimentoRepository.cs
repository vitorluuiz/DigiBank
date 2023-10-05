using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace digibank_back.Interfaces
{
    public interface IInvestimentoRepository
    {
        bool Comprar(Investimento newInvestimento);
        void Post(Investimento newInvestimento, DateTime date);
        bool VenderCotas(int idUsuario, int idOption, decimal qntCotas);
        List<InvestimentoGenerico> GetCarteira(Expression<Func<Investimento, bool>> predicado, int pagina, int qntItens, long minCap = 0, long maxCap = long.MaxValue, Func<Investimento, decimal> ordenador = default, bool desc = false);
        InvestimentoGenerico GetCarteiraItem(Expression<Func<Investimento, bool>> predicado);
        List<InvestimentoGenerico> AllWhere(Expression<Func<Investimento, bool>> where, int pagina, int qntItens, long minCap = 0, long maxCap = long.MaxValue, Func<Investimento, decimal> ordenador = default, bool desc = false, bool isEntrada = true);
        int CountWhere(Expression<Func<Investimento, bool>> where);
        Investimento ListarPorId(int idInvestimento);
        ExtratoInvestimentos ExtratoTotalInvestido(int idUsuario);
        decimal ValorInvestimento(int idUsuario, int idTipoInvestimento, DateTime data);
    }
}
