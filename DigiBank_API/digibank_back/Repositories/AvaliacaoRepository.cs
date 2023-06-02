using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
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
            
            avaliacaoDesatualizada.Nota = avaliacaoAtualizada.Nota;
            avaliacaoDesatualizada.Comentario = avaliacaoAtualizada.Comentario;

            _marketplaceRepository.Atualizar(post);

            ctx.Update(avaliacaoDesatualizada);
            ctx.SaveChanges();
        }

        public bool Cadastrar(Avaliaco newAvaliacao)
        {
            Marketplace post = _marketplaceRepository.ListarPorId((int)newAvaliacao.IdPost, true);
            InventarioRepository _inventarioRepository = new InventarioRepository();

            bool isComprado = _inventarioRepository.VerificaCompra((int)newAvaliacao.IdPost, (int)newAvaliacao.IdUsuario);

            if(!isComprado)
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

            ctx.Avaliacoes.Add(newAvaliacao);
            ctx.SaveChanges();

            return true;
        }

        public void Deletar(int idAvaliacao)
        {
            Avaliaco avaliacao = ListarPorId(idAvaliacao);
            Marketplace post = _marketplaceRepository.ListarPorId((int)avaliacao.IdPost, true);

            decimal somaAvaliacoes = (decimal)((post.Avaliacao * post.QntAvaliacoes) - avaliacao.Nota);
            post.QntAvaliacoes = (short?)(post.QntAvaliacoes - 1);
            post.Avaliacao = somaAvaliacoes / (post.QntAvaliacoes);

            _marketplaceRepository.Atualizar(post);

            ctx.Avaliacoes.Remove(avaliacao);
            ctx.SaveChanges();
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

        public List<Avaliaco> AvaliacoesPost(int idPost, int pagina, int qntItens)
        {
            return ctx.Avaliacoes
                .Where(A => A.IdPost == idPost)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }
    }
}
