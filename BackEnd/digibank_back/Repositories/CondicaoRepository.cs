using digibank_back.Contexts;
using digibank_back.Domains;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class CondicaoRepository : ICondicaoRepository
    {
        digiBankContext ctx = new digiBankContext();
        public Condico Cadastrar(Condico newCondicao)
        {
            ctx.Condicoes.Add(newCondicao);
            ctx.SaveChanges();
            return (newCondicao);
        }

        public void Deletar(int idCondicao)
        {
            ctx.Condicoes.Remove(ctx.Condicoes.Find(idCondicao));
            ctx.SaveChanges();
        }

        public List<Condico> ListarTodas()
        {
            return ctx.Condicoes.ToList();
        }
    }
}
