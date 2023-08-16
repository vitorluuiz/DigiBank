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
        List<InvestimentoGenerico> GetCarteira(int idUsuario, int idTipoInvestimento, int pagina, int qntItens);
        InvestimentoGenerico GetCarteiraItem(int idUsuario, int idOption);
        List<InvestimentoGenerico> AllWhere(Expression<Func<Investimento, bool>> where, int pagina, int qtnItens);
        int CountWhere(Expression<Func<Investimento, bool>> where);
        Investimento ListarPorId(int idInvestimento);
        ExtratoInvestimentos ExtratoTotalInvestido(int idUsuario);
        decimal ValorInvestimento(int idUsuario, int idTipoInvestimento, DateTime data);
    }
}
