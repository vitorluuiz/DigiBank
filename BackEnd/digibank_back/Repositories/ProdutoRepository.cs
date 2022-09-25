using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Atualizar(int idProduto, Produto produtoAtualizado)
        {
            throw new System.NotImplementedException();
        }

        public Produto Cadastrar(Produto newProduto)
        {
            ctx.Produtos.Add(newProduto);
            ctx.SaveChanges();
            return (ctx.Produtos.Find(1));
        }

        public Produto ListarPorId(int idProduto)
        {
            return ctx.Produtos.Find(idProduto);
        }

        public void Deletar(int idProduto)
        {
            ctx.Produtos.Remove(ListarPorId(idProduto));
            ctx.SaveChanges();
        }

        public List<Produto> ListarTodos()
        {
            return ctx.Produtos.ToList();
        }
    }
}
