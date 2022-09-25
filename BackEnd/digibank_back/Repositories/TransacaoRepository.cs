using digibank_back.Domains;
using digibank_back.Interfaces;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        public void DeletarTransacao(int idTransacao)
        {
            throw new System.NotImplementedException();
        }

        public bool EfetuarTransacao(int idUsuarioPagante, int idUsuarioRecebente, float valorFinanceiro)
        {
            throw new System.NotImplementedException();
        }

        public List<Transaco> ListarDeUsuario(int idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public List<Transaco> ListarEntreUsuarios(int idUsuario1, int idUsuario2)
        {
            throw new System.NotImplementedException();
        }

        public Transaco ListarPorid(int idTransacao)
        {
            throw new System.NotImplementedException();
        }

        public List<Transaco> ListarTodas()
        {
            throw new System.NotImplementedException();
        }
    }
}
