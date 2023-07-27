using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IMarketplaceRepository
    {
        Marketplace Cadastrar(Marketplace newPost);
        void Atualizar(Marketplace postAtualizado);
        bool Deletar(int idPost);
        List<PostMinimo> Todos(int pagina, int qntItens);
        List<PostMinimo> TodosPorId(List<int> idsPosts);
        List<PostMinimo> Inativos();
        List<PostMinimo> CompradosAnteriormente(int pagina, int qntItens, int idUsuario);
        Marketplace PorId(int idPost, bool isOwner);
        PostGenerico PublicoPorId(int idPost, bool isOwner);
        List<PostMinimo> PublicoPorUsuario(int idUsuario);
        List<PostMinimo> Meus(int idUsuario);
        bool Comprar(int idComprador, int idPost);
        void TurnInative(int idPost);
        void TurnActive(int idPost);
        List<PostTitle> SearchBestResults(int qntItens);
    }
}
