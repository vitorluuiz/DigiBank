﻿using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IInvestimentoOptionsRepository
    {
        void Cadastrar(InvestimentoOption newInvestimentoOption);
        void Atualizar(int idInvestimentoOption, InvestimentoOption optionAtualizada);
        void Deletar(int idInvestimentoOption);
        List<InvestimentoOption> ListarTodos(int pagina, int qntItens);
        InvestimentoOption ListarPorId(int idInvestimentoOption);
    }
}
