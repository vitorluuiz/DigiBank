using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface ITransacaoRepository
    {
        bool EfetuarTransacao(int idUsuarioPagante, int idUsuarioRecebente, float valorFinanceiro);
        void DeletarTransacao(int idTransacao);
        Transaco ListarPorid(int idTransacao);
        List<Transaco> ListarTodas();
        List<Transaco> ListarDeUsuario(int idUsuario);
        List<Transaco> ListarEntreUsuarios(int idUsuario1, int idUsuario2);
    }
}
