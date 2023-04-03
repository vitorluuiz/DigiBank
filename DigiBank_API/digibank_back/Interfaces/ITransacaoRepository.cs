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
        List<Transaco> ListarTodas(int pagina, int qntItens);
        List<Transaco> ListarEnviadas(int idUsuario, int pagina, int qntItens);
        List<Transaco> ListarRecebidas(int idUsuario, int pagina, int qntItens);
        List<Transaco> ListarEntreUsuarios(int recebente, int pagante, int pagina, int qntItens);
        ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente);
    }
}
