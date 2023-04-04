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
    public class TransacaoRepository : ITransacaoRepository
    {
        private readonly IUsuarioRepository _usuariosRepository;
        public TransacaoRepository()
        {
            _usuariosRepository = new UsuarioRepository();
        }

        digiBankContext ctx = new digiBankContext();

        public void Deletar(int idTransacao)
        {
            ctx.Transacoes.Remove(ListarPorid(idTransacao));
            ctx.SaveChanges();
        }

        public bool EfetuarTransacao(Transaco newTransacao)
        {
            newTransacao.DataTransacao = DateTime.Now;
            Usuario pagante = ctx.Usuarios.FirstOrDefault(u => u.IdUsuario == newTransacao.IdUsuarioPagante);

            bool isSucess = _usuariosRepository.RemoverSaldo(Convert.ToInt16(newTransacao.IdUsuarioPagante), newTransacao.Valor);
            _usuariosRepository.AdicionarSaldo(Convert.ToInt16(newTransacao.IdUsuarioRecebente), newTransacao.Valor);

            if (isSucess)
            {
                ctx.Transacoes.Add(newTransacao);
                ctx.SaveChanges();
                return true;
            }

            return false;
        }

        public List<TransacaoGenerica> ListarRecebidas(int idUsuario, int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == idUsuario)
                .Select(t => new TransacaoGenerica
                {
                    IdTransacao = t.IdTransacao,
                    IdUsuarioPagante = t.IdUsuarioPagante,
                    NomePagante = t.IdUsuarioPaganteNavigation.NomeCompleto,
                    IdUsuarioRecebente = t.IdUsuarioRecebente,
                    NomeRecebente = t.IdUsuarioRecebenteNavigation.NomeCompleto,
                    Valor = t.Valor,
                    DataTransacao = t.DataTransacao,
                    Descricao = t.Descricao
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }
        public List<TransacaoGenerica> ListarEnviadas(int idUsuario, int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioPagante == idUsuario)
                .Select(t => new TransacaoGenerica
                {
                    IdTransacao = t.IdTransacao,
                    IdUsuarioPagante = t.IdUsuarioPagante,
                    NomePagante = t.IdUsuarioPaganteNavigation.NomeCompleto,
                    IdUsuarioRecebente = t.IdUsuarioRecebente,
                    NomeRecebente = t.IdUsuarioRecebenteNavigation.NomeCompleto,
                    Valor = t.Valor,
                    DataTransacao = t.DataTransacao,
                    Descricao = t.Descricao
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public List<TransacaoGenerica> ListarEntreUsuarios(int recebente, int pagante, int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == recebente && t.IdUsuarioPagante == pagante || t.IdUsuarioPagante == recebente && t.IdUsuarioRecebente == pagante)
                .Select(t => new TransacaoGenerica
                {
                    IdTransacao = t.IdTransacao,
                    IdUsuarioPagante = t.IdUsuarioPagante,
                    NomePagante = t.IdUsuarioPaganteNavigation.NomeCompleto,
                    IdUsuarioRecebente = t.IdUsuarioRecebente,
                    NomeRecebente = t.IdUsuarioRecebenteNavigation.NomeCompleto,
                    Valor = t.Valor,
                    DataTransacao = t.DataTransacao,
                    Descricao = t.Descricao
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public List<TransacaoGenerica> ListarEntreUsuarios(int recebente, int pagante)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == recebente && t.IdUsuarioPagante == pagante || t.IdUsuarioPagante == recebente && t.IdUsuarioRecebente == pagante)
                .Select(t => new TransacaoGenerica
                {
                    IdTransacao = t.IdTransacao,
                    IdUsuarioPagante = t.IdUsuarioPagante,
                    NomePagante = t.IdUsuarioPaganteNavigation.NomeCompleto,
                    IdUsuarioRecebente = t.IdUsuarioRecebente,
                    NomeRecebente = t.IdUsuarioRecebenteNavigation.NomeCompleto,
                    Valor = t.Valor,
                    DataTransacao = t.DataTransacao,
                    Descricao = t.Descricao
                })
                .ToList();
        }

        public Transaco ListarPorid(int idTransacao)
        {
            return ctx.Transacoes
                .FirstOrDefault(t => t.IdTransacao == idTransacao);
        }

        public List<TransacaoGenerica> ListarTodas(int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Select(t => new TransacaoGenerica
                {
                    IdTransacao= t.IdTransacao,
                    IdUsuarioPagante = t.IdUsuarioPagante,
                    NomePagante = t.IdUsuarioPaganteNavigation.NomeCompleto,
                    IdUsuarioRecebente = t.IdUsuarioRecebente,
                    NomeRecebente = t.IdUsuarioRecebenteNavigation.NomeCompleto,
                    Valor = t.Valor,
                    DataTransacao = t.DataTransacao,
                    Descricao= t.Descricao
                })
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente)
        {
            List<TransacaoGenerica> listaCompleta =  ListarEntreUsuarios(recebente, pagante);

            ExtratoTransacaoViewModel extrato = new ExtratoTransacaoViewModel();

            extrato.Recebimentos = listaCompleta.Where(t => t.IdUsuarioPagante == recebente).Sum(t => t.Valor);
            extrato.Pagamentos = listaCompleta.Where(t => t.IdUsuarioPagante == pagante).Sum(t => t.Valor) * -1;
            extrato.Saldo = Convert.ToDecimal(extrato.Recebimentos + extrato.Pagamentos);

            return extrato;
        }
    }
}
