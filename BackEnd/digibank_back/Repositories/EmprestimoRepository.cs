using digibank_back.Domains;
using digibank_back.Interfaces;
using System;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        public void AlterarCondicao(int idEmprestimo, int idCondicao)
        {
            throw new NotImplementedException();
        }

        public void Atribuir(Emprestimo newEmprestimo)
        {
            throw new NotImplementedException();
        }

        public void Concluir(int idEmprestimo)
        {
            throw new NotImplementedException();
        }

        public void ConcluirParte(int idEmprestimo, float valor)
        {
            throw new NotImplementedException();
        }

        public void EstenderPrazo(DateTime newPrazo)
        {
            throw new NotImplementedException();
        }

        public List<Emprestimo> ListarDeUsuario(int idUsuario)
        {
            throw new NotImplementedException();
        }

        public Emprestimo ListarPorId(int idEmprestimo)
        {
            throw new NotImplementedException();
        }

        public List<Emprestimo> ListarTodos()
        {
            throw new NotImplementedException();
        }
    }
}
