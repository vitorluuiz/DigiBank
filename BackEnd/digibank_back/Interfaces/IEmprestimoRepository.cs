using digibank_back.Domains;
using System;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IEmprestimoRepository
    {
        void Atribuir(Emprestimo newEmprestimo);
        void Concluir(int idEmprestimo);
        void ConcluirParte(int idEmprestimo, float valor);
        void EstenderPrazo(DateTime newPrazo);
        void AlterarCondicao(int idEmprestimo, int idCondicao);
        Emprestimo ListarPorId(int idEmprestimo);
        List<Emprestimo> ListarTodos();
        List<Emprestimo> ListarDeUsuario(int idUsuario);
    }
}
