using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IAcoesOptionsRepository
    {
        void Cadastrar(Aco newAcao);
        void Atualizar(int idAcao, Aco acaoAtualizada);
        bool Deletar(int idAcao);
        List<Aco> ListarTodas();
        Aco ListarPorId(int idAcao);
        void AlterarDividendos(float dividendos);
        bool DiminuirCotas(int qntCotas);
        void RestaurarCotas(int qntCotas);
        void AlterarIndices(int idAcao, Aco newIndices);
    }
}
