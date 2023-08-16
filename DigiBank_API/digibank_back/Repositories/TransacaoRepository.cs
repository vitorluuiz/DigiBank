﻿using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly digiBankContext _ctx;
        private readonly UsuarioRepository _usuariosRepository;
        public TransacaoRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _usuariosRepository = new UsuarioRepository(ctx, memoryCache);
        }


        public void Deletar(int idTransacao)
        {
            _ctx.Transacoes.Remove(_ctx.Transacoes.FirstOrDefault(t => t.IdTransacao == idTransacao));
            _ctx.SaveChanges();
        }

        public bool EfetuarTransacao(Transaco newTransacao)
        {
            newTransacao.DataTransacao = DateTime.Now;
            Usuario pagante = _ctx.Usuarios.FirstOrDefault(u => u.IdUsuario == newTransacao.IdUsuarioPagante);

            if (newTransacao.IdUsuarioPagante == newTransacao.IdUsuarioRecebente)
            {
                return false;
            }

            bool isSucess =
                _usuariosRepository.CanAddSaldo(newTransacao.IdUsuarioRecebente, newTransacao.Valor)
                && _usuariosRepository.CanRemoveSaldo(newTransacao.IdUsuarioPagante, newTransacao.Valor);

            if (isSucess)
            {
                newTransacao.DataTransacao = DateTime.Now;
                _usuariosRepository.RemoverSaldo(Convert.ToInt16(newTransacao.IdUsuarioPagante), newTransacao.Valor);
                _usuariosRepository.AdicionarSaldo(Convert.ToInt16(newTransacao.IdUsuarioRecebente), newTransacao.Valor);

                _ctx.Transacoes.Add(newTransacao);
                _ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public List<TransacaoGenerica> ListarEntreUsuarios(int recebente, int pagante)
        {
            return _ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == recebente && t.IdUsuarioPagante == pagante || t.IdUsuarioPagante == recebente && t.IdUsuarioRecebente == pagante)
                .Include(t => t.IdUsuarioRecebenteNavigation)
                .Include(t => t.IdUsuarioPaganteNavigation)
                .Select(t => new TransacaoGenerica(t))
                .ToList();
        }

        public ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente)
        {
            List<TransacaoGenerica> listaCompleta = ListarEntreUsuarios(recebente, pagante);

            ExtratoTransacaoViewModel extrato = new ExtratoTransacaoViewModel();

            extrato.Recebimentos = listaCompleta.Where(t => t.IdUsuarioPagante == recebente).Sum(t => t.Valor);
            extrato.Pagamentos = listaCompleta.Where(t => t.IdUsuarioPagante == pagante).Sum(t => t.Valor) * -1;
            extrato.Saldo = Convert.ToDecimal(extrato.Recebimentos + extrato.Pagamentos);

            return extrato;
        }

        public TransacaoCount ListarMinhasTransacoes(int idUsuario, int pagina, int qntItens)
        {
            if (qntItens <= 0)
            {
                throw new ArgumentException("A quantidade de itens por página deve ser maior que zero.", nameof(qntItens));
            }

            TransacaoCount transacoes = new TransacaoCount();
            transacoes.Transacoes = _ctx.Transacoes
                .Where(t => t.IdUsuarioPagante == idUsuario || t.IdUsuarioRecebente == idUsuario)
                .OrderByDescending(t => t.DataTransacao)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();

            transacoes.TransacoesCount = QntTransacoesUsuario(idUsuario);
            return transacoes;

        }

        public int QntTransacoesUsuario(int idUsuario)
        {
            return _ctx.Transacoes
                .Where(t => t.IdUsuarioPagante == idUsuario || t.IdUsuarioRecebente == idUsuario)
                .Count();
        }
    }
}
