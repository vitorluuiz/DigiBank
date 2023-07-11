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
            newPost.IsVirtual = true;

            ctx.Marketplaces.Add(newPost);
            ctx.SaveChanges();

            return newPost;
        }

        public Marketplace ListarPorId(int idPost, bool isOwner)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == true || isOwner == true && p.IsVirtual == true)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefault(p => p.IdPost == idPost);
        }

        public PostGenerico ListarPorIdPublico(int idPost, bool isOwner)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive && p.IsVirtual || isOwner)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    IdUsuario = p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    MainColorHex = p.MainColorHex,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    IsActive = p.IsActive,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao,
                    Imgs = ctx.ImgsPosts
                    .Where(img => img.IdPost == p.IdPost)
                    .Select(img => img.Img)
                    .ToList()
                })
                .FirstOrDefault(p => p.IdPost == idPost);
        }

        public bool Deletar(int idPost)
        {
            Marketplace post = ListarPorId(idPost, true);
            ImgsProdutoRepository imgRepository = new ImgsProdutoRepository();
            AvaliacaoRepository avaliacaoRepository = new AvaliacaoRepository();

            bool isCommentsDeleted = avaliacaoRepository.DeletarFromPost(idPost);

            if (!isCommentsDeleted)
            {
                return false;
            }

            if(post.Vendas == 0)
            {
                foreach(string caminho in imgRepository.ListarCaminhos(Convert.ToByte(idPost)))
                {
                    Upload.RemoverArquivo(caminho);
                }

                imgRepository.DeletarCaminhos(Convert.ToByte(idPost));
                Upload.RemoverArquivo(ctx.Marketplaces.FirstOrDefault(p => p.IdPost == idPost).MainImg);
            }

            post.IsVirtual = false;
            ctx.Update(post);
            ctx.SaveChanges();

            return true;
        }

        public List<PostGenerico> ListarTodos(int pagina, int qntItens)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive == true && p.IsVirtual == true)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    IdUsuario = p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    MainColorHex = p.MainColorHex,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    IsActive = p.IsActive,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao,
                    Imgs = ctx.ImgsPosts
                    .Where(img => img.IdPost == p.IdPost)
                    .Select(img => img.Img)
                    .ToList()
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }

        public bool Comprar(int idComprador, int idPost)
        {
            Marketplace post = ListarPorId(idPost, false);

            if(post.IdUsuario == idComprador)
            {
                return false;
            }

            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Usuario comprador = _usuarioRepository.ListarPorId(idComprador);
            Inventario inventario = new Inventario();

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Compra de {post.Nome} fornecido por {post.IdUsuarioNavigation.NomeCompleto}",
                Valor = post.Valor,
                IdUsuarioPagante = Convert.ToInt16(idComprador),
                IdUsuarioRecebente = Convert.ToInt16(post.IdUsuario)
            };

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if(isSucess)
            {
                    inventario.Valor = post.Valor;
                    inventario.IdPost = post.IdPost;
                    inventario.IdUsuario = comprador.IdUsuario;

                    _inventarioRepository.Depositar(inventario);

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
                .Where(p => p.IsActive == false && p.IsVirtual == true)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    IdUsuario= p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    MainColorHex = p.MainColorHex,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    IsActive = p.IsActive,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao
                })
                .AsNoTracking()
                .ToList();
        }

        public List<PostTitle> SearchBestResults(int qntItens)
        {
            return ctx.Marketplaces
                .Where(p => p.IsActive && p.IsVirtual == true)
                .OrderByDescending(p => p.Avaliacao)
                .Select(p => new PostTitle
                {
                    IdPost = p.IdPost,
                    Titulo = p.Nome,
                    Valor = p.Valor,
                    MainImg = p.MainImg
                })
                .Take(qntItens)
                .ToList();
        }

        public List<PostGenerico> ListarDeUsuarioPublico(int idUsuario)
        {
            return ctx.Marketplaces
                .AsNoTracking()
                .Where(p => p.IdUsuario == idUsuario && p.IsActive == true && p.IsVirtual == true)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    IdUsuario = p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome= p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    MainColorHex = p.MainColorHex,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    IsActive = p.IsActive,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao
                })
                .ToList();
        }

        public List<PostGenerico> ListarMeus(int idUsuario)
        {
            return ctx.Marketplaces
                .Where(p => p.IdUsuario == idUsuario && p.IsVirtual == true)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    IdUsuario = p.IdUsuario,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    Descricao = p.Descricao,
                    MainImg = p.MainImg,
                    MainColorHex = p.MainColorHex,
                    Valor = p.Valor,
                    IsVirtual = p.IsVirtual,
                    IsActive = p.IsActive,
                    Vendas = (short)p.Vendas,
                    QntAvaliacoes = (short)p.QntAvaliacoes,
                    Avaliacao = (decimal)p.Avaliacao
                })
                .ToList();
        }

        public List<PostGenerico> ListarCompradosAnteriormente(int pagina, int qntItens, int idUsuario)
        {
            InventarioRepository inventarioRepository = new InventarioRepository();
            List<PostGenerico> compradosAnteriormente = new List<PostGenerico>();
            HashSet<int> idsAdicionados = new HashSet<int>();
            int paginacao = pagina;
            List<Inventario> inventario = new List<Inventario>();

            do
            {
                inventario = inventarioRepository.ListarMeuInventario(idUsuario, paginacao, qntItens);
                foreach (Inventario item in inventario)
                {
                    if (item.IdPostNavigation.IsVirtual && item.IdPostNavigation.IsActive && !idsAdicionados.Contains(item.IdPost) && idsAdicionados.Count < qntItens)
                    {
                        string apelidoPropritario = ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == item.IdPostNavigation.IdUsuario).Apelido;
                        compradosAnteriormente.Add(new PostGenerico
                        {
                            IdPost = item.IdPost,
                            Nome = item.IdPostNavigation.Nome,
                            ApelidoProprietario = apelidoPropritario,
                            Avaliacao = (decimal)item.IdPostNavigation.Avaliacao,
                            MainColorHex = item.IdPostNavigation.MainColorHex,
                            MainImg = item.IdPostNavigation.MainImg,
                            Valor = item.IdPostNavigation.Valor,
                        });

                        idsAdicionados.Add(item.IdPost);
                    }
                }

                paginacao++;
                //Enquanto eu não tenho a quantidade certa, e não tenho uma lista vazia
            } while (idsAdicionados.Count != qntItens && inventario.Count != 0);

            

            return compradosAnteriormente;
        }

        public List<PostGenerico> ListarTodosPorId(List<int> idsPosts)
        {
            return ctx.Marketplaces
                .Where(p => idsPosts.Contains(p.IdPost))
                .Where(p => p.IsActive && p.IsVirtual)
                .Select(p => new PostGenerico
                {
                    IdPost = p.IdPost,
                    ApelidoProprietario = p.IdUsuarioNavigation.Apelido,
                    Nome = p.Nome,
                    MainImg = p.MainImg,
                    MainColorHex = p.MainColorHex,
                    Valor = p.Valor,
                    Avaliacao = (decimal)p.Avaliacao
                })
                .ToList();
        }
    }
}
