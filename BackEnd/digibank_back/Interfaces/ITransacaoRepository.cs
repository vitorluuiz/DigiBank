using digibank_back.Domains;
using digibank_back.ViewModel;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface ITransacaoRepository
    {
        bool EfetuarTransacao(Transaco newTransacao);
        void Deletar(int idTransacao);
        Transaco ListarPorid(int idTransacao);
        List<Transaco> ListarTodas();
        List<Transaco> ListarEnviadas(int idUsuario);
        List<Transaco> ListarRecebidas(int idUsuario);
        List<Transaco> ListarEntreUsuarios(int recebente, int pagante);
        ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente);
    }
}
