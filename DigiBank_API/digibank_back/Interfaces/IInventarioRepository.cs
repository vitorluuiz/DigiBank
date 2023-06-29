using digibank_back.Domains;
using digibank_back.DTOs;
using System.Collections.Generic;

namespace digibank_back.Interfaces
{
    public interface IInventarioRepository
    {
        List<Inventario> ListarMeuInventario(int idUsuario, int pagina, int qntItens);
        InventarioUser ListarPorId(int idItem);
        void Depositar(Inventario newItem);
        bool Mover(int idItem, int idUsuarioDestino);
        void Deletar(int idItem);
        bool VerificaCompra(int idPost, int idUsuario);
    }
}
