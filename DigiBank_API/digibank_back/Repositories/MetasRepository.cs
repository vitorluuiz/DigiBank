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
        public bool AdicionarMeta(Meta newMeta)
        {
            if(newMeta != null)
            {
                newMeta.Arrecadado = 0;

                ctx.Metas.Add(newMeta);
                ctx.SaveChanges();
                return true;
            }
            return false;
        }

        public bool AdicionarSaldo(int idMeta, decimal amount)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Meta meta = GetMeta(idMeta);

            if(meta == null)
            {
                return false;
            }

            Transaco newTransacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Saldo adicionado à Meta {meta.Titulo} por {meta.IdUsuarioNavigation.Apelido}",
                IdUsuarioPagante = (short)meta.IdUsuario,
                IdUsuarioRecebente = 1
            };

            decimal restante = Convert.ToInt16(meta.ValorMeta - meta.Arrecadado);

            bool isSucessful;
            if(amount >= restante)
            { 
                newTransacao.Valor = restante;
                isSucessful = _transacaoRepository.EfetuarTransacao(newTransacao);
                if(isSucessful)
                {
                    meta.Arrecadado = meta.ValorMeta;
                    ctx.Update(meta);
                    ctx.SaveChanges();
                    return true;
                }
                return false;
            }

            newTransacao.Valor = amount;
            isSucessful = _transacaoRepository.EfetuarTransacao(newTransacao);
            if (isSucessful)
            {
                meta.Arrecadado = meta.Arrecadado + amount;
            
                ctx.Update(meta);
                ctx.SaveChanges();
            
                return true;
            }

            return false;
        }

        public bool AlterarMeta(int idMeta, decimal newValue)
        {
            Meta meta = GetMeta(idMeta);
            if (meta != null)
            {
                meta.ValorMeta = newValue;
                ctx.Update(meta);
                ctx.SaveChanges();

                return true;
            }
            return false;
        }

        public Meta GetMeta(int idMeta)
        {
            return ctx.Metas
                .Include(m => m.IdUsuarioNavigation)
                .FirstOrDefault(m => m.IdMeta == idMeta);
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

        public Meta ListarDestaque(int idUsuario)
        {
            return ctx.Metas
                .Where(i => i.IdUsuario == idUsuario)
                .OrderByDescending(m => m.ValorMeta / (m.Arrecadado > 0 ? m.Arrecadado : 1))
                .FirstOrDefault();
        }

        public void RemoverMeta(int idMeta)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Meta meta = GetMeta(idMeta);
            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Meta concluída por {meta.IdUsuarioNavigation.NomeCompleto}",
                IdUsuarioPagante = 1,
                IdUsuarioRecebente = (short)meta.IdUsuario,
                Valor = (decimal)meta.Arrecadado
            };

            _transacaoRepository.EfetuarTransacao(transacao);

            ctx.Remove(meta);
            ctx.SaveChanges();
        }
    }
}
