using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IMetasRepository
    {
        Meta GetMeta(int idMeta);
        List<Meta> GetMetas();
        List<Meta> GetMinhasMetas(int idUsuario);
        bool AdicionarMeta(Meta newMeta);
        Meta ListarDestaque(int idUsuario);
        bool AlterarMeta(int idMeta, decimal newValue);
        void RemoverMeta(int idMeta);
        bool AdicionarSaldo(int idMeta, decimal amount);
    }
}
