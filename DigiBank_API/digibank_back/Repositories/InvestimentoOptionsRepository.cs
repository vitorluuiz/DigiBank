using digibank_back.Contexts;
using digibank_back.Domains;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class InvestimentoOptionsRepository : IInvestimentoOptionsRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Atualizar(int idInvestimentoOption, InvestimentoOption optionAtualizada)
        {
            InvestimentoOption optionDesatualizada = ListarPorId(idInvestimentoOption);

            optionDesatualizada.Nome = optionAtualizada.Nome;
            optionDesatualizada.IdTipoInvestimento = optionAtualizada.IdTipoInvestimento;
            optionDesatualizada.CodeId = optionAtualizada.CodeId;
            optionDesatualizada.ValorInicial = optionAtualizada.ValorInicial;
            optionDesatualizada.Descricao = optionAtualizada.Descricao;
            optionDesatualizada.Dividendos = optionAtualizada.Dividendos;
            optionDesatualizada.IndiceConfiabilidade = optionAtualizada.IndiceConfiabilidade;
            optionDesatualizada.IndiceDividendos = optionAtualizada.IndiceDividendos;
            optionDesatualizada.IndiceValorizacao = optionAtualizada.IndiceValorizacao;

            ctx.Update(optionDesatualizada);
            ctx.SaveChanges();
        }

        public void Cadastrar(InvestimentoOption newOption)
        {
            ctx.InvestimentoOptions.Add(newOption);
            ctx.SaveChanges();
        }

        public void Deletar(int idInvestimentoOption)
        {
            ctx.Remove(ListarPorId(idInvestimentoOption));
        }

        public InvestimentoOption ListarPorId(int idInvestimentoOption)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimento)
                .AsNoTracking()
                .FirstOrDefault(f => f.IdInvestimentoOption == idInvestimentoOption);
        }

        public List<InvestimentoOption> ListarTodos(int pagina, int qntItens)
        {
            return ctx.InvestimentoOptions
                .Include(f => f.IdTipoInvestimentoNavigation)
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .AsNoTracking()
                .ToList();
        }
    }
}
