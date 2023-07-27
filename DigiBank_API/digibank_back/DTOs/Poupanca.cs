using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace digibank_back.DTOs
{
    public class Poupanca
    {
        public int IdPoupanca { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataAquisicao { get; set; }
        public decimal TotalInvestido { get; set; }
        public decimal Saldo { get; set; }
        public decimal GanhoDiario { get; set; }
        public decimal GanhoMensal { get; set; }
        public decimal GanhoAnual { get; set; }

        public Poupanca(int idUsuario, digiBankContext ctx, IMemoryCache memoryCache)
        {
            PoupancaRepository poupancaRepository = new PoupancaRepository(ctx, memoryCache);
            List<Investimento> depositos = ctx.Investimentos
                .Where(d => d.IdUsuario == idUsuario &&
                    d.IdInvestimentoOption == 1 && d.IsEntrada)
                .ToList();

            if (depositos.Count <= 0) return;
            IdPoupanca = depositos.OrderBy(D => D.DataAquisicao).First().IdInvestimento;
            IdUsuario = idUsuario;
            DataAquisicao = depositos.OrderBy(D => D.DataAquisicao).First().DataAquisicao;
            TotalInvestido = depositos.Sum(D => D.DepositoInicial);
            Saldo = poupancaRepository.Saldo(idUsuario, DateTime.Now);
        }
    }
}
