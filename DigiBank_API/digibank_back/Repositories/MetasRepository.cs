using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class MetasRepository : IMetasRepository
    {
        digiBankContext ctx = new digiBankContext();
        UsuarioRepository _usuarioRepository = new UsuarioRepository();
        public void AdicionarMeta(Meta newMeta)
        {
            if(newMeta != null)
            {
                newMeta.Arrecadado = 0;

                ctx.Metas.Add(newMeta);
                ctx.SaveChanges();
            }
        }

        public bool AdicionarSaldo(int idMeta, decimal amount)
        {
            Meta meta = GetMeta(idMeta);
            if(meta == null)
            {
                return false;
            }

            decimal restante = Convert.ToInt16(meta.ValorMeta - meta.Arrecadado);

            bool isSucessful;
            if(amount >= restante) 
            {
                isSucessful = _usuarioRepository.RemoverSaldo(Convert.ToInt16(meta.IdUsuario), restante);
                if(isSucessful)
                {
                    meta.Arrecadado = meta.ValorMeta;
                    ctx.Update(meta);
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }

            isSucessful = _usuarioRepository.RemoverSaldo(Convert.ToInt16(meta.IdUsuario), amount);
            if (isSucessful)
            {
                meta.Arrecadado = meta.Arrecadado + amount;

                ctx.Update(meta);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public Meta GetMeta(int idMeta)
        {
            return ctx.Metas.FirstOrDefault(m => m.IdMeta == idMeta);
        }

        public List<Meta> GetMetas()
        {
            return ctx.Metas
                .AsNoTracking()
                .ToList();
        }

        public List<Meta> GetMinhasMetas(int idUsuario)
        {
            return ctx.Metas
                .Where(m => m.IdUsuario == idUsuario)
                .AsNoTracking()
                .ToList();
        }

        public void RemoverMeta(int idMeta)
        {
            ctx.Remove(GetMeta(idMeta));
            ctx.SaveChanges();
        }
    }
}
