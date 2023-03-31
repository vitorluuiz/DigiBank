using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IMarketplaceRepository
    {
        Marketplace Cadastrar(Marketplace newPost);
        void Atualizar(Marketplace postAtualizado);
        void Deletar(int idPost);
        List<Marketplace> ListarTodos(int pagina, int qntItens);
        List<Marketplace> ListarInativos();
        Marketplace ListarPorId(int idPost);
        bool Comprar(int idComprador, int idPost);
        void TurnInative(int idPost);
        void TurnActive(int idPost);
    }
}
