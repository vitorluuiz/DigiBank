using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IEmprestimosOptionsRepository
    {
        void Cadastrar(EmprestimosOption newEmprestimosOption);
        void Atualizar(int idEmprestimoOption, EmprestimosOption emprestimoOptionAtualizado);
        void Deletar(int idEmprestimoOption);
        List<EmprestimosOption> ListarTodos();
        EmprestimosOption ListarPorId(int idEmprestimoOption);
    }
}
