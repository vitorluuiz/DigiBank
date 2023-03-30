using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class InventarioRepository : IInventarioRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Depositar(Inventario newItem)
        {
            newItem.DataAquisicao = DateTime.Now;
            ctx.Inventarios.Add(newItem);
            ctx.SaveChanges();
        }

        public void Deletar(int idItem)
        {
            ctx.Inventarios.Remove(ListarPorId(idItem));
        }

        public List<Inventario> ListarMeuInventario(int idUsuario)
        {
            return ctx.Inventarios
                .Where(p => p.IdUsuario == idUsuario)
                .Include(i => i.IdPostNavigation)
                .AsNoTracking()
                .ToList();
        }

        public Inventario ListarPorId(int idInventario)
        {
            return ctx.Inventarios
                .Include(i => i.IdPostNavigation.ImgsPosts)
                .FirstOrDefault(p => p.IdInventario == idInventario);
        }

        public bool Mover(int idItem, int idUsuarioDestino)
        {
            Inventario item = ListarPorId(idItem);
            
            if(item != null && idUsuarioDestino != item.IdUsuario)
            {
                item.DataAquisicao = DateTime.Now;
                item.IdUsuario = Convert.ToInt16(idUsuarioDestino);

                ctx.Update(item);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
