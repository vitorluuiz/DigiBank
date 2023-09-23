using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly digiBankContext _ctx;
        public AreaRepository(digiBankContext ctx)
        {
            _ctx = ctx;
        }

        List<AreaInvestimento> IAreaRepository.GetByTipo(int idTipo)
        {
            return _ctx.AreaInvestimentos.Where(a => a.IdTipoInvestimento == idTipo).ToList();
        }
    }
}
