﻿using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class EmprestimoRepository : IEmprestimoRepository
    {
        digiBankContext ctx = new digiBankContext();
        UsuarioRepository _usuarioRepository = new UsuarioRepository();
        public void AlterarCondicao(int idEmprestimo, int idCondicao)
        {
            Emprestimo emprestimo = ListarPorId(idEmprestimo);

            emprestimo.IdCondicao = Convert.ToByte(idCondicao);

            ctx.Update(emprestimo);
            ctx.SaveChanges();
        }

        public bool Atribuir(Emprestimo newEmprestimo)
        {
            TransacaoRepository _transacaoRepository= new TransacaoRepository();
            Usuario usuario = _usuarioRepository.ListarPorId(Convert.ToInt16(newEmprestimo.IdUsuario));
            EmprestimosOption emprestimoOption = ctx.EmprestimosOptions.FirstOrDefault(o => o.IdEmprestimoOption == newEmprestimo.IdEmprestimoOptions);
            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Aquisição de Empréstimo de {usuario.NomeCompleto}",
                Valor = emprestimoOption.Valor,
                IdUsuarioPagante = 1,
                IdUsuarioRecebente = usuario.IdUsuario
            };

            if (usuario.RendaFixa >= emprestimoOption.RendaMinima && !VerificarAtraso(usuario.IdUsuario) && RetornarQntEmprestimos(usuario.IdUsuario) < 3)
            {
                _transacaoRepository.EfetuarTransacao(transacao);

                newEmprestimo.IdCondicao = 1;
                newEmprestimo.ValorPago = 0;
                newEmprestimo.UltimoPagamento = DateTime.Now;
                newEmprestimo.DataInicial = DateTime.Now;
                newEmprestimo.DataFinal = DateTime.Now.AddDays(emprestimoOption.PrazoEstimado);

                ctx.Emprestimos.Add(newEmprestimo);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public PreviewEmprestimo CalcularPagamento(int idEmprestimo)
        {
            Emprestimo emprestimo = ListarPorId(idEmprestimo);
            EmprestimosOption emprestimoOption = ctx.EmprestimosOptions.FirstOrDefault(o => o.IdEmprestimoOption == emprestimo.IdEmprestimoOptions);
            PreviewEmprestimo previsao = new PreviewEmprestimo();

            TimeSpan diasEmprestados = new TimeSpan();

            if(emprestimo.UltimoPagamento == null)
            {
                diasEmprestados = emprestimo.DataInicial - DateTime.Now;
            }
            else
            {
                diasEmprestados = (TimeSpan)(emprestimo.UltimoPagamento - DateTime.Now);
            }

            previsao.TaxaJuros = emprestimoOption.TaxaJuros;
            previsao.DiasEmprestados = diasEmprestados.Days;

            if(emprestimo.ValorPago != null)
            {
                previsao.Valor = (decimal)(emprestimoOption.Valor - emprestimo.ValorPago);
            }
            else
            {
                previsao.Valor = emprestimoOption.Valor;
            }

            previsao.PagamentoPrevisto = previsao.Valor + (previsao.Valor * (previsao.DiasEmprestados / 30) * (previsao.TaxaJuros / 100));

            return previsao;
        }

        public bool Concluir(int idEmprestimo)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Emprestimo emprestimo = ListarPorId(idEmprestimo);
            Usuario usuario = _usuarioRepository.ListarPorId(Convert.ToInt16(emprestimo.IdUsuario));
            PreviewEmprestimo previsao = CalcularPagamento(idEmprestimo);
            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Empréstimo de {usuario.NomeCompleto} concluído",
                Valor = previsao.Valor,
                IdUsuarioPagante = usuario.IdUsuario,
                IdUsuarioRecebente = 1
            };

            bool isSucess = _transacaoRepository.EfetuarTransacao(transacao);

            if (isSucess)
            {
                AlterarCondicao(idEmprestimo, 2);

                return true;
            }

            return false;
        }

        public bool ConcluirParte(int idEmprestimo, decimal valor)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository();
            Emprestimo emprestimo = ListarPorId(idEmprestimo);
            Usuario usuario = _usuarioRepository.ListarPorId(Convert.ToUInt16(emprestimo.IdUsuario));
            PreviewEmprestimo previsao = CalcularPagamento(idEmprestimo);
            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Parcela de empréstimo pago por {usuario.NomeCompleto}",
                IdUsuarioPagante = usuario.IdUsuario,
                IdUsuarioRecebente = 1
            };

            if(usuario.Saldo >= previsao.PagamentoPrevisto)
            {
                if (previsao.PagamentoPrevisto >= valor)
                {
                    transacao.Valor = previsao.PagamentoPrevisto;
                    _transacaoRepository.EfetuarTransacao(transacao);

                    emprestimo.ValorPago = emprestimo.ValorPago + valor;

                    AlterarCondicao(emprestimo.IdEmprestimo, 2);
                }
                else
                {
                    transacao.Valor = valor;
                    _transacaoRepository.EfetuarTransacao(transacao);

                    emprestimo.ValorPago = emprestimo.ValorPago + previsao.PagamentoPrevisto;

                    AlterarCondicao(idEmprestimo, 2);
                }

                emprestimo.UltimoPagamento = DateTime.Now;

                ctx.Update(emprestimo);
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public void EstenderPrazo( int idEmprestimo, DateTime newPrazo)
        {
            Emprestimo emprestimo = ListarPorId(idEmprestimo);

            emprestimo.DataFinal = newPrazo;

            ctx.Update(emprestimo);
            ctx.SaveChanges();
        }

        public List<Emprestimo> ListarDeUsuario(int idUsuario)
        {
            return ctx.Emprestimos
                .Where(e => e.IdUsuario == idUsuario && e.IdCondicao != 2)
                .Include(e => e.IdEmprestimoOptionsNavigation)
                .AsNoTracking()
                .ToList();
        }

        public Emprestimo ListarPorId(int idEmprestimo)
        {
            return ctx.Emprestimos.FirstOrDefault(e => e.IdEmprestimo == idEmprestimo);
        }

        public List<Emprestimo> ListarTodos()
        {
            return ctx.Emprestimos
                .AsNoTracking()
                .ToList();
        }

        public int RetornarQntEmprestimos(int idUsuario)
        {
            return ListarDeUsuario(idUsuario).Count();
        }

        public bool VerificarAtraso(int idUsuario)
        {
            List<Emprestimo> pendencias = ListarDeUsuario(idUsuario).Where(e => e.IdCondicao == 3 || e.DataFinal < DateTime.Now).ToList();

            if(pendencias.Count() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
