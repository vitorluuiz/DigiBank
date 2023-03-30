using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class MarketplaceRepository : IMarketplaceRepository
    {
        digiBankContext ctx = new digiBankContext();
        UsuarioRepository _usuarioRepository = new UsuarioRepository();
        InventarioRepository _inventarioRepository = new InventarioRepository();
        public void Atualizar(int idPost, Marketplace produtoAtualizado)
        {
            throw new System.NotImplementedException();
        }

        public Marketplace Cadastrar(Marketplace newPost)
        {
            ctx.Marketplaces.Add(newPost);

            if(newPost.IsVirtual == null)
            {
                newPost.IsVirtual = true;
            }

            newPost.IsVisible = true;

            ctx.SaveChanges();

            return newPost;
        }

        public Marketplace ListarPorId(int idPost)
        {
            return ctx.Marketplaces
                .Include(p => p.IdUsuarioNavigation)
                .Include(p => p.Avaliacos)
                .Include(p => p.ImgsPosts)
                .FirstOrDefault(p => p.IdPost == idPost);
        }

        public void Deletar(int idPost)
        {
            ImgsProdutoRepository imgRepository = new ImgsProdutoRepository();

            foreach(string caminho in imgRepository.ListarCaminhos(Convert.ToByte(idPost)))
            {
                Upload.RemoverArquivo(caminho);
            }

            imgRepository.DeletarCaminhos(Convert.ToByte(idPost));
            Upload.RemoverArquivo(ctx.Marketplaces.FirstOrDefault(p => p.IdPost == idPost).MainImg);

            ctx.Marketplaces.Remove(ListarPorId(idPost));
            ctx.SaveChanges();
        }

        public List<Marketplace> ListarTodos()
        {
            return ctx.Marketplaces
                .Include(p => p.IdUsuarioNavigation)
                .Where(p => p.IsVisible == true)
                .AsNoTracking()
                .ToList();
        }

        public bool Comprar(int idComprador, int idPost)
        {
            Usuario comprador = _usuarioRepository.ListarPorId(idComprador);
            Marketplace post = ListarPorId(idPost);
            Inventario inventario = new Inventario();

            if(comprador.Saldo >= post.Valor)
            {
                _usuarioRepository.RemoverSaldo(Convert.ToInt16(idComprador), post.Valor);
                _usuarioRepository.AdicionarSaldo(Convert.ToInt16(post.IdUsuario), post.Valor);

                if ((bool)post.IsVirtual)
                {
                    inventario.Valor = post.Valor;
                    inventario.IdPost = post.IdPost;
                    inventario.IdUsuario = comprador.IdUsuario;

                    _inventarioRepository.Depositar(inventario);
                }

                return true;
            }
            return false;
        }

        public void TurnInvisible(int idPost)
        {
            Marketplace visiblePost = ListarPorId(idPost);
            visiblePost.IsVisible = false;
            
            ctx.Update(visiblePost);
            ctx.SaveChanges();
        }

        public void TurnVisible(int idPost)
        {
            Marketplace visiblePost = ListarPorId(idPost);
            visiblePost.IsVisible = true;

            ctx.Update(visiblePost);
            ctx.SaveChanges();
        }

        public List<Marketplace> ListarInvisibles()
        {
            return ctx.Marketplaces
                .Include(p => p.IdUsuarioNavigation)
                .Where(p => p.IsVisible == false)
                .AsNoTracking()
                .ToList();
        }
    }
}
