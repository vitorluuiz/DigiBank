using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.ViewModel.Transacao;
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

        public List<Transaco> ListarRecebidas(int idUsuario, int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == idUsuario)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }
        public List<Transaco> ListarEnviadas(int idUsuario, int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioPagante == idUsuario)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public List<Transaco> ListarEntreUsuarios(int recebente, int pagante, int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == recebente && t.IdUsuarioPagante == pagante || t.IdUsuarioPagante == recebente && t.IdUsuarioRecebente == pagante)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public List<Transaco> ListarEntreUsuarios(int recebente, int pagante)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == recebente && t.IdUsuarioPagante == pagante || t.IdUsuarioPagante == recebente && t.IdUsuarioRecebente == pagante)
                .ToList();
        }

        public Transaco ListarPorid(int idTransacao)
        {
            return ctx.Transacoes
                .FirstOrDefault(t => t.IdTransacao == idTransacao);
        }

        public List<Transaco> ListarTodas(int pagina, int qntItens)
        {
            return ctx.Transacoes
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }

        public ExtratoTransacaoViewModel FluxoTotal(int pagante, int recebente)
        {
            List<Transaco> listaCompleta =  ListarEntreUsuarios(recebente, pagante);

            ExtratoTransacaoViewModel extrato = new ExtratoTransacaoViewModel();

            extrato.Recebimentos = listaCompleta.Where(t => t.IdUsuarioPagante == recebente).Sum(t => t.Valor);
            extrato.Pagamentos = listaCompleta.Where(t => t.IdUsuarioPagante == pagante).Sum(t => t.Valor) * -1;
            extrato.Saldo = Convert.ToDecimal(extrato.Recebimentos + extrato.Pagamentos);

            return extrato;
        }
    }
}
