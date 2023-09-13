using digibank_back.Domains;
using digibank_back.DTOs;
using System;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface ITransacaoRepository
    {
        bool EfetuarTransacao(Transaco newTransacao);
        void Deletar(int idTransacao);
        int QntTransacoesUsuario(int idUsuario);
        TransacaoCount ListarMinhasTransacoes(int idUsuario, int pagina, int qntItens);
        ExtratoTransacaoViewModel GetFluxoFromDate(int idUsuario, DateTime start);
        List<TransacaoGenerica> ListarEntreUsuarios(int recebente, int pagante);
        ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente);
    }
}
