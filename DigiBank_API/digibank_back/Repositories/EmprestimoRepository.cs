using digibank_back.Contexts;
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
    public class EmprestimoRepository : IEmprestimoRepository
    {
        private readonly digiBankContext _ctx;
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IMemoryCache _memoryCache;

        public EmprestimoRepository(digiBankContext ctx, UsuarioRepository usuarioRepository, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _usuarioRepository = usuarioRepository;
            _memoryCache = memoryCache;
        }

        public void AlterarCondicao(int idEmprestimo, int idCondicao)
        {
            Emprestimo emprestimo = ListarPorId(idEmprestimo);

            emprestimo.IdCondicao = Convert.ToByte(idCondicao);

            _ctx.Update(emprestimo);
            _ctx.SaveChanges();
        }

        public bool Atribuir(Emprestimo newEmprestimo)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository(_ctx, _memoryCache);
            Usuario usuario = _usuarioRepository.PorId(Convert.ToInt16(newEmprestimo.IdUsuario));
            EmprestimosOption emprestimoOption = _ctx.EmprestimosOptions.FirstOrDefault(o => o.IdEmprestimoOption == newEmprestimo.IdEmprestimoOptions);
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

                _ctx.Emprestimos.Add(newEmprestimo);
                _ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool ConcluirParte(int idEmprestimo, decimal valor)
        {
            TransacaoRepository _transacaoRepository = new TransacaoRepository(_ctx, _memoryCache);
            Emprestimo emprestimo = ListarPorId(idEmprestimo);
            EmprestimoSimulado simulacao = new(emprestimo);
            Usuario usuario = _usuarioRepository.PorId(Convert.ToUInt16(emprestimo.IdUsuario));
            Transaco transacao = new Transaco
            {
                DataTransacao = DateTime.Now,
                Descricao = $"Parcela de empréstimo pago por {usuario.NomeCompleto}",
                IdUsuarioPagante = usuario.IdUsuario,
                IdUsuarioRecebente = 1
            };

            if (usuario.Saldo >= valor)
            {
                decimal restanteArredondado = Math.Round(simulacao.RestanteAvista, 2);
                if (restanteArredondado > valor)
                {
                    transacao.Valor = valor;
                    _transacaoRepository.EfetuarTransacao(transacao);

                    emprestimo.ValorPago += valor;
                }
                else
                {
                    transacao.Valor = restanteArredondado;
                    _transacaoRepository.EfetuarTransacao(transacao);

                    emprestimo.ValorPago += restanteArredondado;

                    AlterarCondicao(idEmprestimo, 2);
                }

                emprestimo.UltimoPagamento = DateTime.Now;

                _ctx.Update(emprestimo);
                _ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public void EstenderPrazo(int idEmprestimo, DateTime newPrazo)
        {
            Emprestimo emprestimo = ListarPorId(idEmprestimo);

            emprestimo.DataFinal = newPrazo;

            _ctx.Update(emprestimo);
            _ctx.SaveChanges();
        }

        public List<Emprestimo> ListarDeUsuario(int idUsuario)
        {
            return _ctx.Emprestimos
                .Where(e => e.IdUsuario == idUsuario && e.IdCondicao != 2)
                .Include(e => e.IdEmprestimoOptionsNavigation)
                .Include(e => e.IdCondicaoNavigation)
                .AsNoTracking()
                .ToList();
        }

        public Emprestimo ListarPorId(int idEmprestimo)
        {
            return _ctx.Emprestimos
                .Include(e => e.IdEmprestimoOptionsNavigation)
                .Include(e => e.IdCondicaoNavigation)
                .FirstOrDefault(e => e.IdEmprestimo == idEmprestimo);
        }

        public int RetornarQntEmprestimos(int idUsuario)
        {
            return ListarDeUsuario(idUsuario).Count();
        }

        public EmprestimoSimulado Simular(int idEmprestimo)
        {
            return new EmprestimoSimulado(ListarPorId(idEmprestimo));
        }

        public bool VerificarAtraso(int idUsuario)
        {
            List<Emprestimo> pendencias = ListarDeUsuario(idUsuario).Where(e => e.IdCondicao == 3 || e.DataFinal < DateTime.Now).ToList();

            return pendencias.Any();
        }
    }
}
