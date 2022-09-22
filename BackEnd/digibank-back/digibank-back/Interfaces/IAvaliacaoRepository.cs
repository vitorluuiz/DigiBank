using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IAvaliacaoRepository
    {
        void Cadastrar(Avaliaco newAvaliacao);
        void Deletar(int idAvaliacao);
        List<Avaliaco> ListarTodas();
        List<Avaliaco> ListarTodasDoProduto(int idProduto);
    }
}
