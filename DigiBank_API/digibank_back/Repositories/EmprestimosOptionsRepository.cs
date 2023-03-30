﻿using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class EmprestimosOptionsRepository : IEmprestimosOptionsRepository
    {
        digiBankContext ctx = new digiBankContext();
        public void Atualizar(int idEmprestimoOption, EmprestimosOption emprestimoOptionAtualizado)
        {
            EmprestimosOption optionDesatualizada = ListarPorId(idEmprestimoOption);

            optionDesatualizada.PrazoEstimado = emprestimoOptionAtualizado.PrazoEstimado;
            optionDesatualizada.TaxaJuros = emprestimoOptionAtualizado.TaxaJuros;
            optionDesatualizada.Valor = emprestimoOptionAtualizado.Valor;

            ctx.Update(optionDesatualizada);
            ctx.SaveChanges();
        }

        public void Cadastrar(EmprestimosOption newEmprestimosOption)
        {
            ctx.EmprestimosOptions.Add(newEmprestimosOption);
            ctx.SaveChanges();
        }

        public PreviewEmprestimo CalcularPrevisao(EmprestimosOption emprestimo)
        {
            PreviewEmprestimo previsao = new PreviewEmprestimo();

            previsao.TaxaJuros = emprestimo.TaxaJuros;
            previsao.Valor = emprestimo.Valor;
            previsao.DiasEmprestados = emprestimo.PrazoEstimado;

            previsao.PagamentoPrevisto = previsao.Valor + (previsao.Valor * (previsao.DiasEmprestados/30) * (previsao.TaxaJuros/100));

            return previsao;
        }

        public void Deletar(int idEmprestimoOption)
        {
            ctx.EmprestimosOptions.Remove(ListarPorId(idEmprestimoOption));
            ctx.SaveChanges();
        }

        public EmprestimosOption ListarPorId(int idEmprestimoOption)
        {
            return ctx.EmprestimosOptions.FirstOrDefault(e => e.IdEmprestimoOption == idEmprestimoOption);
        }

        public List<EmprestimosOption> ListarTodos()
        {
            return ctx.EmprestimosOptions
                .AsNoTracking()
                .ToList();
        }
    }
}
