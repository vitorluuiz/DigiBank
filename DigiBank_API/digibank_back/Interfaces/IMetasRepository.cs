using digibank_back.Domains;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IMetasRepository
    {
        Meta GetMeta(int idMeta);
        List<Meta> GetMetas();
        List<Meta> GetMinhasMetas(int idUsuario);
<<<<<<< HEAD
        bool AdicionarMeta(Meta newMeta);
=======
        Meta ListarDestaque(int idUsuario);
        void AdicionarMeta(Meta newMeta);
>>>>>>> 027de580ce16ceae759ca9bef32c0f631909fb7b
        void RemoverMeta(int idMeta);
        bool AdicionarSaldo(int idMeta, decimal amount);
    }
}
