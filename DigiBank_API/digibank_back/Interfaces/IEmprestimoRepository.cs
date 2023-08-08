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
        void EstenderPrazo(int idEmprestimo, DateTime newPrazo);
        void AlterarCondicao(int idEmprestimo, int idCondicao);
        PreviewEmprestimo CalcularPagamento(int idEmprestimo);
        int RetornarQntEmprestimos(int idUsuario);
        bool VerificarAtraso(int idUsuario);
        Emprestimo ListarPorId(int idEmprestimo);
        List<Emprestimo> ListarDeUsuario(int idUsuario);
    }
}
