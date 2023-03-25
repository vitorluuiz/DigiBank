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

        public void AtualizarAvaliacao(int idAvaliacao, Avaliaco avaliacaoAtualizada)
        {
            Avaliaco avaliacaoDesatualizada = ListarPorId(idAvaliacao);

            avaliacaoDesatualizada.Nota = avaliacaoAtualizada.Nota;
            avaliacaoDesatualizada.Comentario = avaliacaoAtualizada.Comentario;

            ctx.Update(avaliacaoDesatualizada);
            ctx.SaveChanges();
        }

        public void Cadastrar(Avaliaco newAvaliacao)
        {
            ctx.Avaliacoes.Add(newAvaliacao);
            ctx.SaveChanges();
        }

        public void Deletar(int idAvaliacao)
        {
            ctx.Avaliacoes.Remove(ListarPorId(idAvaliacao));
            ctx.SaveChanges();
        }

        public Avaliaco ListarPorId(int idAvaliacao)
        {
            return ctx.Avaliacoes.FirstOrDefault(a => a.IdAvaliacao == idAvaliacao);
        }

        public List<Avaliaco> ListarTodas()
        {
            return ctx.Avaliacoes
                .AsNoTracking()
                .ToList();
        }

        public List<Avaliaco> ListarTodasDoProduto(int idProduto)
        {
            return ctx.Avaliacoes
                .AsNoTracking()
                .ToList()
                .Where(A => A.IdProduto == idProduto).ToList();
        }
    }
}
