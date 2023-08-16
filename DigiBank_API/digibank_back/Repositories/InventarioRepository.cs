using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class InventarioRepository : IInventarioRepository
    {
        private readonly digiBankContext _ctx;

        public InventarioRepository(digiBankContext ctx)
        {
            _ctx = ctx;
        }

        public void Depositar(Inventario newItem)
        {
            newItem.DataAquisicao = DateTime.Now;
            _ctx.Inventarios.Add(newItem);
            _ctx.SaveChanges();
        }

        public void Deletar(int idItem)
        {
            _ctx.Inventarios.Remove(_ctx.Inventarios.FirstOrDefault(i => i.IdInventario == idItem));
            _ctx.SaveChanges();
        }

        public List<Inventario> Meu(int idUsuario, int pagina, int qntItens)
        {
            return _ctx.Inventarios
                .Where(p => p.IdUsuario == idUsuario)
                .Include(i => i.IdPostNavigation)
                .OrderBy(p => p.DataAquisicao)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public InventarioUser PorId(int idInventario)
        {
            return _ctx.Inventarios
                .Include(p => p.IdPostNavigation.ImgsPosts)
                .Select(i => new InventarioUser
                {
                    IdInventario = i.IdInventario,
                    IdUsuario = i.IdUsuario,
                    DataAquisicao = i.DataAquisicao,
                    Imgs = _ctx.ImgsPosts
                    .Where(img => img.IdPost == i.IdPost)
                    .Select(img => img.Img)
                    .ToList()
                })
                .FirstOrDefault(p => p.IdInventario == idInventario);
        }

        public bool VerificaCompra(int idPost, int idUsuario)
        {
            Inventario comprado = _ctx.Inventarios.FirstOrDefault(i => i.IdPost == idPost && i.IdUsuario == idUsuario);

            return comprado != null;
        }
    }
}
