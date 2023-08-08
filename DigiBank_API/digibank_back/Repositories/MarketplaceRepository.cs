using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace digibank_back.Repositories
{
    public class MarketplaceRepository : IMarketplaceRepository
    {
        private readonly digiBankContext _ctx;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly InventarioRepository _inventarioRepository;
        private readonly IMemoryCache _memoryCache;
        public MarketplaceRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _inventarioRepository = new InventarioRepository(ctx);
            _usuarioRepository = new UsuarioRepository(ctx, memoryCache);
            _memoryCache = memoryCache;
        }
        public void Atualizar(Marketplace postAtualizado)
        {
            Marketplace post = PorId(postAtualizado.IdPost, true);

            post.Avaliacao = postAtualizado.Avaliacao;
            post.QntAvaliacoes = postAtualizado.QntAvaliacoes;

            _ctx.Update(post);
            _ctx.SaveChanges();
        }

        public Marketplace Cadastrar(Marketplace newPost)
        {
            newPost.Vendas = 0;
            newPost.Avaliacao = 0;
            newPost.QntAvaliacoes = 0;
            newPost.IsActive = true;
            newPost.IsVirtual = true;

            _ctx.Marketplaces.Add(newPost);
            _ctx.SaveChanges();

            return newPost;
        }

        public Marketplace PorId(int idPost, bool isOwner)
        {
            return _ctx.Marketplaces
                .Where(p => (p.IsActive == true || (isOwner == true && p.IsVirtual == true)) && p.IdPost == idPost)
                .Include(p => p.IdUsuarioNavigation)
                .FirstOrDefault();
        }

        public PostGenerico PublicoPorId(int idPost, bool isOwner)
        {
            Marketplace post = _ctx.Marketplaces.FirstOrDefault(p => p.IdPost == idPost && (p.IsActive || isOwner) && p.IsVirtual);

            if (post != null)
            {
                List<string> imgs = _ctx.ImgsPosts
                    .Where(img => img.IdPost == idPost)
                    .Select(img => img.Img)
                    .ToList();

                return new PostGenerico(post, imgs);
            }

            return null;
        }

        public bool Deletar(int idPost)
        {
            Marketplace post = PorId(idPost, true);
            ImgsProdutoRepository imgRepository = new ImgsProdutoRepository();
            AvaliacaoRepository avaliacaoRepository = new AvaliacaoRepository(_ctx, _memoryCache);

            bool isCommentsDeleted = avaliacaoRepository.DeletarFromPost(idPost);

            if (!isCommentsDeleted)
            {
                return false;
            }

            if (post.Vendas == 0)
            {
                foreach (string caminho in imgRepository.ListarCaminhos(Convert.ToByte(idPost)))
                {
                    Upload.RemoverArquivo(caminho);
                }

                imgRepository.DeletarCaminhos(Convert.ToByte(idPost));
                Upload.RemoverArquivo(_ctx.Marketplaces.FirstOrDefault(p => p.IdPost == idPost)?.MainImg);
            }

            post.IsVirtual = false;
            _ctx.Update(post);
            _ctx.SaveChanges();

            return true;
        }

        public List<PostMinimo> Todos(int pagina, int qntItens)
        {
            return _ctx.Marketplaces
                .Where(p => p.IsActive == true && p.IsVirtual == true)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public bool Comprar(int idComprador, int idPost)
        {
            var post = PorId(idPost, false);

            if (post.IdUsuario == idComprador)
            {
                return false;
            }

            TransacaoRepository _transacaoRepository = new(_ctx, _memoryCache);
            var comprador = _usuarioRepository.PorId(idComprador);
            var inventario = new Inventario();

            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Compra de {post.Nome} fornecido por {post.IdUsuarioNavigation.NomeCompleto}",
                Valor = post.Valor,
                IdUsuarioPagante = Convert.ToInt16(idComprador),
                IdUsuarioRecebente = Convert.ToInt16(post.IdUsuario)
            };

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if (!isSucess) return false;
            inventario.Valor = post.Valor;
            inventario.IdPost = post.IdPost;
            inventario.IdUsuario = comprador.IdUsuario;

            _inventarioRepository.Depositar(inventario);

            post.Vendas++;

            _ctx.Update(post);
            _ctx.SaveChanges();

            return true;
        }

        public void TurnInative(int idPost)
        {
            Marketplace activePost = PorId(idPost, true);
            activePost.IsActive = false;

            _ctx.Update(activePost);
            _ctx.SaveChanges();
        }

        public void TurnActive(int idPost)
        {
            Marketplace inativePost = PorId(idPost, true);
            inativePost.IsActive = true;

            _ctx.Update(inativePost);
            _ctx.SaveChanges();
        }

        public List<PostMinimo> Inativos()
        {
            return _ctx.Marketplaces
                .Where(p => p.IsActive == false && p.IsVirtual == true)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .ToList();
        }

        public List<PostTitle> SearchBestResults(int qntItens)
        {
            return _ctx.Marketplaces
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

        public List<PostMinimo> PublicoPorUsuario(int idUsuario)
        {
            return _ctx.Marketplaces
                .Where(p => p.IdUsuario == idUsuario && p.IsActive == true && p.IsVirtual == true)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .ToList();
        }

        public List<PostMinimo> Meus(int idUsuario)
        {
            return _ctx.Marketplaces.Where(p => p.IdUsuario == idUsuario && p.IsVirtual == true)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .ToList();
        }

        public List<PostMinimo> CompradosAnteriormente(int pagina, int qntItens, int idUsuario)
        {
            InventarioRepository inventarioRepository = new(new digiBankContext());
            int paginacao = pagina;
            List<int> idsAdicionados = new();
            int itensDisponiveis = 1;

            do
            {
                List<Inventario> inventario = inventarioRepository.Meu(idUsuario, paginacao, qntItens);
                foreach (Inventario item in inventario)
                {
                    if (item.IdPostNavigation.IsVirtual && item.IdPostNavigation.IsActive && !idsAdicionados.Contains(item.IdPost) && idsAdicionados.Count < qntItens)
                    {
                        idsAdicionados.Add(item.IdPost);
                    }
                }

                itensDisponiveis = inventario.Count;
                paginacao++;
            } while (idsAdicionados.Count != qntItens && itensDisponiveis != 0);

            return TodosPorId(idsAdicionados);
        }

        public List<PostMinimo> TodosPorId(List<int> idsPosts)
        {
            return _ctx.Marketplaces
                .Where(p => idsPosts.Contains(p.IdPost) && p.IsVirtual && p.IsActive)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .ToList();
        }

        public List<PostMinimo> AllOrderBy(Expression<Func<Marketplace, decimal>> filter, int pagina, int qntItens)
        {
            return _ctx.Marketplaces
                .OrderBy(filter)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .ToList();
        }

        public List<PostMinimo> AllWhere(Expression<Func<Marketplace, bool>> predicate, int pagina, int qntItens)
        {
            return _ctx.Marketplaces
                .Where(predicate)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .Include(p => p.IdUsuarioNavigation)
                .Select(p => new PostMinimo(p))
                .ToList();
        }
    }
}