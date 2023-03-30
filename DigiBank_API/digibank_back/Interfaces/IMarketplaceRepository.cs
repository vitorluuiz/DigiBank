using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IMarketplaceRepository
    {
        Marketplace Cadastrar(Marketplace newPost);
        void Atualizar(int idPost, Marketplace postAtualizado);
        void Deletar(int idPost);
        List<Marketplace> ListarTodos();
        List<Marketplace> ListarInvisibles();
        Marketplace ListarPorId(int idPost);
        bool Comprar(int idComprador, int idPost);
        void TurnInvisible(int idPost);
        void TurnVisible(int idPost);
    }
}
