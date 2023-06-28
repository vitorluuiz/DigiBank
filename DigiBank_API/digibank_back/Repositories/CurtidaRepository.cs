using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class CurtidaRepository : ICurtidaRepository
    {
        digiBankContext ctx = new digiBankContext();
        public bool Cadastrar(Curtida like)
        {
            if (!HasSameReplie(like) && !HasSameOrigin(like))
            {
                ctx.Curtidas.Add(like);
                ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public bool Deletar(int idCurtida, int idUsuario)
        {
            Curtida replie = ctx.Curtidas.FirstOrDefault(C => C.IdCurtida == idCurtida && C.IdUsuario == idUsuario);

            if (replie == null)
            {
                return false;
            }

            ctx.Curtidas.Remove(ctx.Curtidas.Find(idCurtida));
            ctx.SaveChanges();

            return true;
        }

        public bool DeletarFromComment(int idComment)
        {
            Avaliaco avaliacao = ctx.Avaliacoes.FirstOrDefault(A => A.IdAvaliacao == idComment);

            if (avaliacao == null)
            {
                return false;
            }

            List<Curtida> curtidas = ctx.Curtidas.Where(C => C.IdAvaliacao == idComment).ToList();

            ctx.RemoveRange(curtidas);
            ctx.SaveChanges();

            return true;
        }

        public bool HasSameOrigin(Curtida like)
        {
            Avaliaco avaliacao = ctx.Avaliacoes.FirstOrDefault(A => A.IdAvaliacao == like.IdAvaliacao);

            if (avaliacao.IdUsuario == like.IdUsuario)
            {
                return true;
            }

            return false;
        }

        public bool HasSameReplie(Curtida like)
        {
            if (ctx.Curtidas.FirstOrDefault(C => C.IdAvaliacao == like.IdAvaliacao && C.IdUsuario == like.IdUsuario) == null)
            {
                return false;
            }

            return true;
        }
    }
}
