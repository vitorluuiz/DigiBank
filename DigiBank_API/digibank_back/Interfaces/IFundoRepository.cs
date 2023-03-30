using digibank_back.Domains;
using digibank_back.ViewModel;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IFundoRepository
    {
        bool Comprar(Fundo newFundo, int idComprador);
        void Vender(int idFundo, int idVendedor);
        Fundo ListarPorId(int idFundo);
        List<Fundo> ListarTodos();
        List<Fundo> ListarDeUsuario(int idUsuario);
        PreviewRentabilidade CalcularPrevisao(int idFundo, decimal diasInvestidos);
    }
}
