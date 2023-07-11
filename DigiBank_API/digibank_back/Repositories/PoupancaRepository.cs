using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class PoupancaRepository : IPoupancaRepository
    {
        digiBankContext ctx = new digiBankContext();
        public decimal CalcularGanhos(int idUsuario, DateTime inicio, DateTime fim)
        {
            List<Investimento> depositos = ctx.Investimentos
                .Where(D => D.IdUsuario == idUsuario &&
                D.IdInvestimentoOptionNavigation.IdTipoInvestimento == 5)
                .ToList();

            throw new NotImplementedException();
        }

        public bool Depositar(int idUsuario, decimal quantidade)
        {
            Usuario usuario = ctx.Usuarios.FirstOrDefault(U => U.IdUsuario == idUsuario);
            if (usuario == null) { return false; }
            UsuarioRepository usuarioRepository = new UsuarioRepository();
            
            if (usuarioRepository.CanRemoveSaldo(idUsuario, quantidade))
            {
                usuarioRepository.RemoverSaldo((short)idUsuario, quantidade);
                ctx.Investimentos.Add(new Investimento
                {
                    IdInvestimentoOption = 1,
                    IdUsuario = idUsuario,
                    DepositoInicial = quantidade,
                    DataAquisicao = DateTime.Now,
                    QntCotas = 1,
                });
                ctx.SaveChanges();

                return true;
            }

            return false;
        }

        public bool Sacar(int idUsuario, decimal quantidade)
        {
            throw new NotImplementedException();
        }
    }
}
