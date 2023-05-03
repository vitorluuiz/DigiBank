using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IInvestimentoRepository
    {
        bool Comprar(Investimento newInvestimento, int idComprador);
        void Vender(int idInvestimento);
        void VenderCotas(int idInvestimento, decimal qntCotas);
        Investimento ListarPorId(int idInvestimento);
        List<Investimento> ListarTodos();
        List<Investimento> ListarDeUsuario(int idUsuario);
        decimal RetornarInvestimentoTotal(int idUsuario);
        PreviewRentabilidade CalcularGanhos(int idInvestimento);
        PreviewRentabilidade CalcularPrevisao(int idInvestimento, decimal diasInvestidos);
    }
}
