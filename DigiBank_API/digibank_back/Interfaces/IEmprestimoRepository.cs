using digibank_back.Domains;
using digibank_back.ViewModel;
using System;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IEmprestimoRepository
    {
        bool Atribuir(Emprestimo newEmprestimo);
        bool Concluir(int idEmprestimo);
        bool ConcluirParte(int idEmprestimo, decimal valor);
        void EstenderPrazo( int idEmprestimo, DateTime newPrazo);
        void AlterarCondicao(int idEmprestimo, int idCondicao);
        PreviewEmprestimo CalcularPagamento(int idEmprestimo);
        Emprestimo ListarPorId(int idEmprestimo);
        List<Emprestimo> ListarTodos();
        List<Emprestimo> ListarDeUsuario(int idUsuario);
    }
}
