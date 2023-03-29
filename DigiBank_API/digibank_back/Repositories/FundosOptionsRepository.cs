using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class FundosOptionsRepository : IFundosOptionsRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Atualizar(int idFundoOption, FundosOption fundoAtualizado)
        {
            FundosOption fundoDesatualizado = ListarPorId(idFundoOption);

            fundoDesatualizado.TaxaJuros = fundoAtualizado.TaxaJuros;
            fundoDesatualizado.NomeFundo = fundoAtualizado.NomeFundo;
            fundoDesatualizado.IndiceConfiabilidade = fundoAtualizado.IndiceConfiabilidade;
            fundoDesatualizado.IndiceDividendos = fundoAtualizado.IndiceDividendos;
            fundoDesatualizado.IndiceValorizacao = fundoAtualizado.IndiceValorizacao;
            fundoDesatualizado.IdTipoFundo = fundoAtualizado.IdTipoFundo;

            ctx.Update(fundoDesatualizado);
            ctx.SaveChanges();
        }

        public void Cadastrar(FundosOption newFundoOption)
        {
            ctx.FundosOptions.Add(newFundoOption);
        }

        public void Deletar(int idFundoOption)
        {
            ctx.Remove(ListarPorId(idFundoOption));
        }

        public FundosOption ListarPorId(int idFundoOption)
        {
            return ctx.FundosOptions
                .Include(f => f.IdTipoFundoNavigation)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdFundosOption == idFundoOption);
        }

        public List<FundosOption> ListarTodos()
        {
            return ctx.FundosOptions
                .Include(f => f.IdTipoFundoNavigation)
                .AsNoTracking()
                .ToList();
        }
    }
}
