using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Utils;
using Microsoft.EntityFrameworkCore;
using System;
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
            return (newProduto);
        }

        public Produto ListarPorId(int idProduto)
        {
            return ctx.Produtos
                .Include(p => p.IdUsuarioNavigation)
                .Include(p => p.Avaliacos)
                .Include(p => p.ImgsProdutos)
                .FirstOrDefault(p => p.IdProduto == idProduto);
        }

        public void Deletar(int idProduto)
        {
            ImgsProdutoRepository imgRepository = new ImgsProdutoRepository();

            foreach(string caminho in imgRepository.ListarCaminhos(Convert.ToByte(idProduto)))
            {
                Upload.RemoverArquivo(caminho);
            }

            imgRepository.DeletarCaminhos(Convert.ToByte(idProduto));
            Upload.RemoverArquivo(ctx.Produtos.FirstOrDefault(p => p.IdProduto == idProduto).ProdutoImg);
            ctx.Produtos.Remove(ListarPorId(idProduto));
            ctx.SaveChanges();
        }

        public List<Produto> ListarTodos()
        {
            return ctx.Produtos
                .Include(p => p.IdUsuarioNavigation)
                .AsNoTracking()
                .ToList();
        }

        public bool Comprar(int idComprador, int idProduto)
        {
            throw new System.NotImplementedException();
        }
    }
}
