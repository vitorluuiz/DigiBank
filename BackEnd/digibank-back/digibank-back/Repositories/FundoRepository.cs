using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using System.Collections.Generic;

namespace digibank_back.Repositories
{
    public class FundoRepository : IFundoRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Comprar(Fundo newFundo)
        {
            throw new System.NotImplementedException();
        }

        public List<Fundo> ListarDeOption(int idOption)
        {
            throw new System.NotImplementedException();
        }

        public List<Fundo> ListarDeUsuario(int idUsuario)
        {
            throw new System.NotImplementedException();
        }

        public Fundo ListarPorId(int idFundo)
        {
            return ctx.Fundos.Find(idFundo);
        }

        public List<Fundo> ListarTodos()
        {
            throw new System.NotImplementedException();
        }

        public void Vender(int idFundo)
        {
            throw new System.NotImplementedException();
        }
    }
}
