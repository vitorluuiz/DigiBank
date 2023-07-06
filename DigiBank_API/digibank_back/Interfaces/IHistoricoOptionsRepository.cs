using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IHistoricoOptionsRepository
    {
        void UpdateHistory(int idOption);
        List<HistoricoInvestimentoOption> GetHistoryFromOption(int idOption);
    }
}
