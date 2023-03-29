using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class FundoRepository : IFundoRepository
    {
        digiBankContext ctx = new digiBankContext();
        private UsuarioRepository _usuarioRepository = new UsuarioRepository();

        public PreviewRentabilidade CalcularPrevisao(int idFundo, int diasInvestidos)
        {
            Fundo fundo = ListarPorId(idFundo);
            PreviewRentabilidade previsao = new PreviewRentabilidade();
            previsao.DepositoInicial = fundo.DepositoInicial;
            previsao.MontanteTotal = fundo.DepositoInicial + (fundo.DepositoInicial * (diasInvestidos / 30) * (1 + fundo.IdfundosOptionsNavigation.TaxaJuros/100));
            previsao.GanhosPrevistos = previsao.MontanteTotal - previsao.DepositoInicial;
            previsao.TaxaJuros = fundo.IdfundosOptionsNavigation.TaxaJuros;
            previsao.diasInvestidos = diasInvestidos;

            return previsao;
        }

        public bool Comprar(Fundo newFundo, int idComprador)
        {
            bool isSucess = _usuarioRepository.RemoverSaldo(Convert.ToInt16(idComprador), newFundo.DepositoInicial);

            if(isSucess )
            {
                ctx.Fundos.Add(newFundo);
                ctx.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Fundo> ListarDeUsuario(int idUsuario)
        {
            return ctx.Fundos
                .Where(f => f.IdUsuario == idUsuario)
                .Include(f => f.IdfundosOptionsNavigation)
                .AsNoTracking()
                .ToList();
        }

        public Fundo ListarPorId(int idFundo)
        {
            return ctx.Fundos
                .Include(f => f.IdfundosOptionsNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdFundo == idFundo);
        }

        public List<Fundo> ListarTodos()
        {
            return ctx.Fundos
                .AsNoTracking()
                .ToList();
        }

        public void Vender(int idFundo, int idVendedor)
        {
            Fundo fundoVendido = ListarPorId(idFundo);
            TimeSpan diasInvestidos = fundoVendido.DataInicio - DateTime.Now;
            decimal valorGanho = fundoVendido.DepositoInicial + (fundoVendido.DepositoInicial * (Convert.ToInt16(diasInvestidos.TotalDays /30)) * (1 + fundoVendido.IdfundosOptionsNavigation.TaxaJuros/100));

            _usuarioRepository.AdicionarSaldo(idVendedor, valorGanho);

            ctx.Fundos.Remove(fundoVendido);
            ctx.SaveChanges();
        }
    }
}
