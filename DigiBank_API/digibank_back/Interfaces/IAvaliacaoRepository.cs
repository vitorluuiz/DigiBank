using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IAvaliacaoRepository
    {
        void Cadastrar(Avaliaco newAvaliacao);
        void Deletar(int idAvaliacao);
        List<Avaliaco> ListarTodas();
        Avaliaco ListarPorId(int idAvaliacao);
        List<Avaliaco> AvaliacoesPost(int idProduto);
        void AtualizarAvaliacao(int idAvaliacao, Avaliaco avaliacaoAtualizada);
    }
}
