using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IAvaliacaoRepository
    {
        bool Cadastrar(Avaliaco newAvaliacao);
        void Deletar(int idAvaliacao);
        List<Avaliaco> ListarTodas(int pagina, int qntItens);
        Avaliaco ListarPorId(int idAvaliacao);
        List<Avaliaco> AvaliacoesPost(int idProduto, int pagina, int qntItens);
        List<AvaliacoesHist> CountAvaliacoesRating(int idProduto); 
        void AtualizarAvaliacao(int idAvaliacao, Avaliaco avaliacaoAtualizada);
    }
}
