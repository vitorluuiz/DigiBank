using digibank_back.Contexts;
using digibank_back.Domains;
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

        public List<Transaco> ListarRecebidas(int idUsuario)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioRecebente == idUsuario)
                .ToList();
        }
        public List<Transaco> ListarEnviadas(int idUsuario)
        {
            return ctx.Transacoes
                .Where(t => t.IdUsuarioPagante == idUsuario)
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

        public List<Transaco> ListarTodas()
        {
            return ctx.Transacoes
                .ToList();
        }

        public List<decimal> FluxoTotal(int pagante, int recebente)
        {
            List<Transaco> listaCompleta =  ListarEntreUsuarios(recebente, pagante);

            decimal recebimentos = listaCompleta.Where(t => t.IdUsuarioPagante == recebente).Sum(t => t.Valor);
            decimal pagamentos = listaCompleta.Where(t => t.IdUsuarioPagante == pagante).Sum(t => t.Valor) * -1;
            decimal total = recebimentos + pagamentos;

            List<decimal> resultado = new List<decimal>();

            resultado.Add(pagamentos);
            resultado.Add(recebimentos);
            resultado.Add(total);

            return resultado;
        }
    }
}
