using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IInventarioRepository
    {
        List<Inventario> Meu(int idUsuario, int pagina, int qntItens);
        InventarioUser PorId(int id);
        void Depositar(Inventario newItem);
        void Deletar(int idItem);
        bool VerificaCompra(int idPost, int idUsuario);
    }
}
