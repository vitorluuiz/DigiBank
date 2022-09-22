using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IAcaoRepository
    {
        void Comprar(Aco newAcao);
        void VenderTudo(int idAcao);
        void VenderCotas(int idAcao, int qntCotas);
        Aco ListarPorId(int idAcao);
        List<Aco> ListarTodas();
        List<Aco> ListarDeOption(int idOption);
        List<Aco> ListarDeUsuario(int idUsuario);
    }
}
