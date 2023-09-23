using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IAreaRepository
    {
        List<AreaInvestimento> GetByTipo(int idTipo);
    }
}
