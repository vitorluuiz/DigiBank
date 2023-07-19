using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Interfaces
{
    public interface IHistoryInvestRepository
    {
        void UpdateOptionHistory(int idOption);
        List<HistoricoInvestimentoOption> GetHistoryFromOption(int idOption, int days);
        List<HistoricoTotalInvestido> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim);
        ////IEnumerable<IGrouping<int, HistoricoTotalInvestido>> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim);
        decimal GetOptionValue(int idOption, DateTime data);
    }
}
