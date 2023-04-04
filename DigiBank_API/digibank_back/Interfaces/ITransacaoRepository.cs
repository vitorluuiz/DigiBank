using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface ITransacaoRepository
    {
        bool EfetuarTransacao(Transaco newTransacao);
        void Deletar(int idTransacao);
        Transaco ListarPorid(int idTransacao);
        List<TransacaoGenerica> ListarTodas(int pagina, int qntItens);
        List<TransacaoGenerica> ListarEnviadas(int idUsuario, int pagina, int qntItens);
        List<TransacaoGenerica> ListarRecebidas(int idUsuario, int pagina, int qntItens);
        List<TransacaoGenerica> ListarEntreUsuarios(int recebente, int pagante, int pagina, int qntItens);
        ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente);
    }
}
