using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IMarketplaceRepository
    {
        Marketplace Cadastrar(Marketplace newPost);
        void Atualizar(Marketplace postAtualizado);
        void Deletar(int idPost);
        List<PostGenerico> ListarTodos(int pagina, int qntItens);
        List<PostGenerico> ListarInativos();
        Marketplace ListarPorId(int idPost, bool isOwner);
        PostGenerico ListarPorIdPublico(int idPost, bool isOwner);
        bool Comprar(int idComprador, int idPost);
        void TurnInative(int idPost);
        void TurnActive(int idPost);
        List<PostTitle> SearchBestResults(int qntItens);
    }
}
