using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
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
            Marketplace post = ListarPorId(postAtualizado.IdPost, true);

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

        public Marketplace ListarPorId(int idPost, bool isOwner)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == true || isOwner == true)
                .FirstOrDefault(p => p.IdPost == idPost);
        }

        public PostGenerico ListarPorIdPublico(int idPost, bool isOwner)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == true || isOwner == true)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    Idusuario = p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao
                })
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

            ctx.Marketplaces.Remove(ListarPorId(idPost, true));
            ctx.SaveChanges();
        }

        public List<PostGenerico> ListarTodos(int pagina, int qntItens)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == true)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }

        public bool Comprar(int idComprador, int idPost)
        {
            Usuario comprador = _usuarioRepository.ListarPorId(idComprador);
            Marketplace post = ListarPorId(idPost, true);
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
            Marketplace visiblePost = ListarPorId(idPost, true);
            visiblePost.IsActive = false;
            
            ctx.Update(visiblePost);
            ctx.SaveChanges();
        }

        public void TurnActive(int idPost)
        {
            Marketplace visiblePost = ListarPorId(idPost, true);
            visiblePost.IsActive = true;

            ctx.Update(visiblePost);
            ctx.SaveChanges();
        }

        public List<PostGenerico> ListarInativos()
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == false)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    Idusuario= p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao
                })
                .AsNoTracking()
                .ToList();
        }
    }
}
