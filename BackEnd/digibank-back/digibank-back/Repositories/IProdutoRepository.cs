using digibank_back.Domains;

namespace digibank_back.Repositories
{
    public interface IProdutoRepository
    {
        void Cadastrar(Produto newProduto);
        void Atualizar(int idProduto, Produto produtoAtualizado);
        void Deletar(int idProduto);
    }
}
