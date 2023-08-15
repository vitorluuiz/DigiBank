using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.Repositories
{
    public class EmprestimosOptionsRepository : IEmprestimosOptionsRepository
    {
        private readonly digiBankContext _ctx;
        private readonly IMemoryCache _memoryCache;

        public EmprestimosOptionsRepository(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _memoryCache = memoryCache;
        }

        public void Cadastrar(EmprestimosOption newEmprestimosOption)
        {
            _ctx.EmprestimosOptions.Add(newEmprestimosOption);
            _ctx.SaveChanges();
        }

        public PreviewEmprestimo CalcularPrevisao(EmprestimosOption emprestimo)
        {
            PreviewEmprestimo previsao = new PreviewEmprestimo();

            previsao.TaxaJuros = emprestimo.TaxaJuros;
            previsao.Valor = emprestimo.Valor;
            previsao.DiasEmprestados = emprestimo.PrazoEstimado;

            previsao.PagamentoPrevisto = previsao.Valor + (previsao.Valor * (previsao.DiasEmprestados / 30) * (previsao.TaxaJuros / 100));

            return previsao;
        }

        public void Deletar(int idEmprestimoOption)
        {
            _ctx.EmprestimosOptions.Remove(ListarPorId(idEmprestimoOption));
            _ctx.SaveChanges();
        }

        public List<EmprestimosOption> ListarDisponiveis(int idUsuario, int pagina, int qntItens)
        {
            UsuarioRepository _usuarioRepository = new UsuarioRepository(_ctx, _memoryCache);
            Usuario usuario = _usuarioRepository.PorId(idUsuario);

            if (usuario != null)
            {
                return _ctx.EmprestimosOptions
                    .AsNoTracking()
                    .Where(o => o.RendaMinima <= usuario.RendaFixa)
                    .Skip((pagina - 1) * qntItens)
                    .Take(qntItens)
                    .ToList();
            }

            return null;
        }

        public EmprestimosOption ListarPorId(int idEmprestimoOption)
        {
            return _ctx.EmprestimosOptions.FirstOrDefault(e => e.IdEmprestimoOption == idEmprestimoOption);
        }

        public EmprestimoSimulado Simular(int idEmprestimoOption)
        {
            return new EmprestimoSimulado(ListarPorId(idEmprestimoOption));
        }
    }
}
