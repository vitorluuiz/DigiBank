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
        public void Atualizar(Marketplace postAtualizado)
        {
            Marketplace post = ListarPorId(postAtualizado.IdPost);

            post.Avaliacao = postAtualizado.Avaliacao;
            post.QntAvaliacoes = postAtualizado.QntAvaliacoes;

            ctx.Update(post);
            ctx.SaveChanges();
        }

        public Marketplace Cadastrar(Marketplace newPost)
        {
            newPost.Vendas = 0;
            newPost.Avaliacao = 0;
            newPost.QntAvaliacoes = 0;
            newPost.IsActive = true;

            ctx.Marketplaces.Add(newPost);
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

        public List<Marketplace> ListarTodos(int pagina, int qntItens)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == true)
                .Include(p => p.IdUsuarioNavigation)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
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

                if (post.IsVirtual)
                {
                    inventario.Valor = post.Valor;
                    inventario.IdPost = post.IdPost;
                    inventario.IdUsuario = comprador.IdUsuario;

                    _inventarioRepository.Depositar(inventario);
                }

                post.Vendas++;

                ctx.Update(post);
                ctx.SaveChanges();

                return true;
            }
            return false;
        }

        public void TurnInative(int idPost)
        {
            Marketplace visiblePost = ListarPorId(idPost);
            visiblePost.IsActive = false;
            
            ctx.Update(visiblePost);
            ctx.SaveChanges();
        }

        public void TurnActive(int idPost)
        {
            Marketplace visiblePost = ListarPorId(idPost);
            visiblePost.IsActive = true;

            ctx.Update(visiblePost);
            ctx.SaveChanges();
        }

        public List<Marketplace> ListarInativos()
        {
            return ctx.Marketplaces
                .Include(p => p.IdUsuarioNavigation)
                .Where(p => p.IsActive == false)
                .AsNoTracking()
                .ToList();
        }
    }
}
