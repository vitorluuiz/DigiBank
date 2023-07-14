using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IHistoryInvestRepository
    {
        void UpdateOptionHistory(int idOption);
        List<HistoricoInvestimentoOption> GetHistoryFromOption(int idOption, int days);
        List<HistoricoTotalInvestido> GetHistoryFromInvest(int idUsuario, DateTime inicio, DateTime fim);
    }
}
