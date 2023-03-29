using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public class AcoesOptionsRepository : IAcoesOptionsRepository
    {
        public void AlterarDividendos(float dividendos)
        {
            throw new System.NotImplementedException();
        }

        public void AlterarIndices(int idAcao, Aco newIndices)
        {
            throw new System.NotImplementedException();
        }

        public void Atualizar(int idAcao, Aco acaoAtualizada)
        {
            throw new System.NotImplementedException();
        }

        public void Cadastrar(Aco newAcao)
        {
            throw new System.NotImplementedException();
        }

        public bool Deletar(int idAcao)
        {
            throw new System.NotImplementedException();
        }

        public bool DiminuirCotas(int qntCotas)
        {
            throw new System.NotImplementedException();
        }

        public Aco ListarPorId(int idAcao)
        {
            throw new System.NotImplementedException();
        }

        public List<Aco> ListarTodas()
        {
            throw new System.NotImplementedException();
        }

        public void RestaurarCotas(int qntCotas)
        {
            throw new System.NotImplementedException();
        }
    }
}
