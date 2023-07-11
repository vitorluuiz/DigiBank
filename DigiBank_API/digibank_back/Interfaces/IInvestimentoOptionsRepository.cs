using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public interface IInvestimentoOptionsRepository
    {
        InvestimentoOption CreateFicOption();
        void Atualizar(int idInvestimentoOption, InvestimentoOption optionAtualizada);
        void Deletar(int idInvestimentoOption);
        List<InvestimentoOptionGenerico> ListarTodos(int pagina, int qntItens);
        List<InvestimentoOptionGenerico> ListarCompradosAnteriormente(int pagina, int qntItens, int idUsuario);
        InvestimentoOption ListarPorId(int idInvestimentoOption);
        List<InvestimentoTitle> BuscarInvestimentos(int qntItens);
        List<InvestimentoOptionGenerico> ListarPorTipoInvestimento(byte idTipoInvestimentoOption, int pagina, int qntItens);
        bool ComprarOption(int idComprador, int idPost);
    }
}
