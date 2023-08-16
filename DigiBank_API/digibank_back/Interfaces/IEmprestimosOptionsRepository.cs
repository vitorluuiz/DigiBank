﻿using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IEmprestimosOptionsRepository
    {
        void Cadastrar(EmprestimosOption newEmprestimosOption);
        void Deletar(int idEmprestimoOption);
        List<EmprestimosOption> ListarDisponiveis(int idUsuario, int pagina, int qntItens);
        PreviewEmprestimo CalcularPrevisao(EmprestimosOption emprestimo);
        EmprestimosOption ListarPorId(int idEmprestimoOption);
    }
}
