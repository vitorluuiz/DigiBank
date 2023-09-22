using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IEmprestimoRepository
    {
        bool Atribuir(Emprestimo newEmprestimo);
        bool ConcluirParte(int idEmprestimo, decimal valor);
        bool EstenderPrazo(Emprestimo emprestimo);
        void AlterarCondicao(int idEmprestimo, int idCondicao);
        EmprestimoSimulado Simular(int idEmprestimo);
        bool CanEstender(int idEmprestimo);
        int RetornarQntEmprestimos(int idUsuario);
        bool VerificarAtraso(int idUsuario);
        Emprestimo ListarPorId(int idEmprestimo);
        List<Emprestimo> ListarDeUsuario(int idUsuario);
    }
}
