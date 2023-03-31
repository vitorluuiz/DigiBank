using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class TipoInvestimentoRepository : ITipoInvestimentoRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Cadastrar(TipoInvestimento newTipoInvestimento)
        {
            ctx.TipoInvestimentos.Add(newTipoInvestimento);
            ctx.SaveChanges();
        }

        public void Deletar(int idTipoInvestimento)
        {
            ctx.TipoInvestimentos.Remove(ctx.TipoInvestimentos.FirstOrDefault(t => t.IdTipoInvestimento ==  idTipoInvestimento));
            ctx.SaveChanges();
        }

        public List<TipoInvestimento> ListarTodos()
        {
            return ctx.TipoInvestimentos
                .AsNoTracking()
                .ToList();
        }
    }
}
