using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IProdutoRepository
    {
        Produto Cadastrar(Produto newProduto);
        void Atualizar(int idProduto, Produto produtoAtualizado);
        void Deletar(int idProduto);
        List<Produto> ListarTodos();
        Produto ListarPorId(int idProduto);
        bool Comprar(int idComprador, int idProduto);
    }
}
