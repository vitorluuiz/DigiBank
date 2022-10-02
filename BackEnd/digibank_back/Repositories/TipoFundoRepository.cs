using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class TipoFundoRepository : ITipoFundoRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Cadastrar(TiposFundo newFundo)
        {
            ctx.TiposFundos.Add(newFundo);
            ctx.SaveChanges();
        }

        public void Deletar(int idFundo)
        {
            ctx.TiposFundos.Remove(ctx.TiposFundos.FirstOrDefault(t => t.IdTipoFundo == idFundo));
            ctx.SaveChanges();
        }

        public List<TiposFundo> ListarTodos()
        {
            return ctx.TiposFundos.AsNoTracking().ToList();
        }
    }
}
