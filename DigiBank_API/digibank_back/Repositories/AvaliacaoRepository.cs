using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class AvaliacaoRepository : IAvaliacaoRepository
    {
        digiBankContext ctx = new digiBankContext();
        MarketplaceRepository _marketplaceRepository = new MarketplaceRepository();

        public void AtualizarAvaliacao(int idAvaliacao, Avaliaco avaliacaoAtualizada)
        {
            Avaliaco avaliacaoDesatualizada = ListarPorId(idAvaliacao);
            Marketplace post = _marketplaceRepository.ListarPorId((int)avaliacaoDesatualizada.IdPost, true);

            decimal somaAvaliacoes = (decimal)((post.QntAvaliacoes - 1) * (post.Avaliacao * post.QntAvaliacoes - avaliacaoDesatualizada.Nota));
            post.Avaliacao = (somaAvaliacoes + avaliacaoAtualizada.Nota) / post.QntAvaliacoes;
            
            avaliacaoDesatualizada.DataPostagem = DateTime.Now;
            avaliacaoDesatualizada.Nota = avaliacaoAtualizada.Nota;
            avaliacaoDesatualizada.Comentario = avaliacaoAtualizada.Comentario;

            _marketplaceRepository.Atualizar(post);

            ctx.Update(avaliacaoDesatualizada);
            ctx.SaveChanges();
        }

        public bool Cadastrar(Avaliaco newAvaliacao)
        {
            Marketplace post = _marketplaceRepository.ListarPorId((int)newAvaliacao.IdPost, true);

            bool hasCommentRights = HasCommentsRights((int)newAvaliacao.IdUsuario, (int)newAvaliacao.IdPost);

            if(!hasCommentRights)
            {
                return false;
            }

            if(post.Avaliacao == 0 || post.QntAvaliacoes == 0)
            {
                post.Avaliacao = newAvaliacao.Nota;
            }
            else
            {
                post.Avaliacao = ((post.QntAvaliacoes * post.Avaliacao) + newAvaliacao.Nota) / (post.QntAvaliacoes + 1);

            }

            post.QntAvaliacoes = (short?)(post.QntAvaliacoes + 1);

            _marketplaceRepository.Atualizar(post);
            newAvaliacao.DataPostagem = DateTime.Now;
            newAvaliacao.Replies = 0;

            ctx.Avaliacoes.Add(newAvaliacao);
            ctx.SaveChanges();

            return true;
        }

        public bool Deletar(int idAvaliacao)
        {
            CurtidaRepository curtidaRepository = new CurtidaRepository();

            bool isSucess = curtidaRepository.DeletarFromComment(idAvaliacao);

            if (!isSucess)
            {
                return false;
            }

            Avaliaco avaliacao = ListarPorId(idAvaliacao);
            Marketplace post = _marketplaceRepository.ListarPorId((int)avaliacao.IdPost, true);

            decimal somaAvaliacoes = (decimal)((post.Avaliacao * post.QntAvaliacoes) - avaliacao.Nota);
            post.QntAvaliacoes = (short?)(post.QntAvaliacoes - 1);

            if(somaAvaliacoes != 0 && post.QntAvaliacoes != 0)
            {
                post.Avaliacao = somaAvaliacoes / (post.QntAvaliacoes);
            }
            else
            {
                post.Avaliacao = 0;
                post.QntAvaliacoes = 0;
                _marketplaceRepository.Atualizar(post);
            }

            _marketplaceRepository.Atualizar(post);

            ctx.Avaliacoes.Remove(avaliacao);
            ctx.SaveChanges();

            return true;
        }

        public Avaliaco ListarPorId(int idAvaliacao)
        {
            return ctx.Avaliacoes.FirstOrDefault(a => a.IdAvaliacao == idAvaliacao);
        }

        public List<Avaliaco> ListarTodas(int pagina, int qntItens)
        {
            return ctx.Avaliacoes
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }

        public List<AvaliacaoSimples> AvaliacoesPost(int idPost, int idUsuario, int pagina, int qntItens)
        {
            return ctx.Avaliacoes
                .Where(A => A.IdPost == idPost)
                .OrderByDescending(A => A.Replies)
                .OrderBy(A => A.DataPostagem)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .Select(A => new AvaliacaoSimples 
                { 
                    IdPost = (byte)A.IdPost,
                    IdAvaliacao = A.IdAvaliacao,
                    IdUsuario = (int)A.IdUsuario,
                    DataPostagem = A.DataPostagem,
                    Comentario = A.Comentario,
                    Nota = A.Nota,
                    Replies = A.Replies,
                    Publicador = A.IdUsuarioNavigation.Apelido,
                    IsReplied = (ctx.Curtidas.FirstOrDefault(C => C.IdAvaliacao == A.IdAvaliacao && C.IdUsuario == idUsuario) != null)
                })
                .AsNoTracking()
                .ToList();
        }

        public List<AvaliacoesHist> CountAvaliacoesRating(int idProduto)
        {
            List<Avaliaco> postAvaliacoes = ctx.Avaliacoes.Where(A => A.IdPost == idProduto).ToList();
            List<AvaliacoesHist> ratingHistograma = new List<AvaliacoesHist>
            {
                new AvaliacoesHist
                {
                    Indice = 5,
                    Count = postAvaliacoes.Where(A => A.Nota == 5).Count(),
                },
                new AvaliacoesHist
                {
                    Indice = 4,
                    Count = postAvaliacoes.Where(A => A.Nota == 4).Count(),
                },
                new AvaliacoesHist
                {
                    Indice = 3,
                    Count = postAvaliacoes.Where(A => A.Nota == 3).Count(),
                },
                new AvaliacoesHist
                {
                    Indice = 2,
                    Count = postAvaliacoes.Where(A => A.Nota == 2).Count(),
                },
                new AvaliacoesHist
                {
                    Indice = 1,
                    Count = postAvaliacoes.Where(A => A.Nota == 1).Count(),
                },
            };

            return ratingHistograma;
        }

        public bool HasCommentsRights(int idUsuario, int idPost)
        {
            InventarioRepository _inventarioRepository = new InventarioRepository();
            bool isComprado = _inventarioRepository.VerificaCompra(idPost, idUsuario);

            if(ctx.Avaliacoes.FirstOrDefault(A => A.IdPost == idPost && A.IdUsuario == idUsuario) != null)
            {
                return false;
            }

            return isComprado;
        }

        public bool AddLike(int idAvaliacao, int idUsuario)
        {
            CurtidaRepository _curtidaRepository = new CurtidaRepository();
            Curtida newLike = new Curtida
            {
                IdAvaliacao = idAvaliacao,
                IdUsuario = idUsuario,
            };

            Avaliaco avaliacao = ListarPorId(idAvaliacao);

            bool isSucess = _curtidaRepository.Cadastrar(newLike);

            if(isSucess && avaliacao != null)
            {
                avaliacao.Replies++;
                ctx.Update(avaliacao);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool RemoveLike(int idAvaliacao, int idUsuario)
        {
            CurtidaRepository _curtidaRepository = new CurtidaRepository();

            Curtida like = ctx.Curtidas.FirstOrDefault(C => C.IdAvaliacao == idAvaliacao && C.IdUsuario == idUsuario);

            if(like == null)
            {
                return false;
            }

            Avaliaco comment = ListarPorId(like.IdAvaliacao);

            bool isSucess = _curtidaRepository.Deletar(like.IdCurtida, idUsuario);

            if (isSucess)
            {
                comment.Replies--;
                ctx.Update(comment);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool DeletarFromPost(int idPost)
        {
            Marketplace post = ctx.Marketplaces.FirstOrDefault(M => M.IdPost == idPost);

            if (post == null)
            {
                return false;
            }

            if(post.QntAvaliacoes <= 0)
            {
                return true;
            }

            List<AvaliacaoSimples> avaliacoes = AvaliacoesPost(idPost, 0, 1, (int)post.QntAvaliacoes);

            if (avaliacoes.Count <= 0)
            {
                return true;
            }

            foreach (AvaliacaoSimples avaliacao in avaliacoes)
            {
                bool isDeleted = Deletar(avaliacao.IdAvaliacao);

                if (!isDeleted)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
