using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
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

        public List<EmprestimosOption> ListarDisponiveis(int idUsuario, int pagina, int qntItens)
        {
            UsuarioRepository _usuarioRepository = new UsuarioRepository();
            Usuario usuario = _usuarioRepository.ListarPorId(idUsuario);

            if (usuario != null)
            {
                return ctx.EmprestimosOptions
                    .AsNoTracking()
                    .Where(o => o.RendaMinima < usuario.RendaFixa)
                    .Skip((pagina - 1) * qntItens)
                    .Take(qntItens)
                    .ToList();
            }

            return null;
        }

        public EmprestimosOption ListarPorId(int idEmprestimoOption)
        {
            return ctx.EmprestimosOptions.FirstOrDefault(e => e.IdEmprestimoOption == idEmprestimoOption);
        }

        public List<EmprestimosOption> ListarTodos(int pagina, int qntItens)
        {
            return ctx.EmprestimosOptions
                .AsNoTracking()
                .Skip((pagina - 1) * qntItens)
                .Take(qntItens)
                .ToList();
        }
    }
}
