﻿using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class InvestimentoOptionsController : ControllerBase
    {
        private readonly digiBankContext _ctx;
        private readonly IInvestimentoOptionsRepository _investimentoOptionsRepository;
        private readonly IMemoryCache _memoryCache;
        public InvestimentoOptionsController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _investimentoOptionsRepository = new InvestimentoOptionsRepository(ctx, memoryCache);
            _memoryCache = memoryCache;
        }

        [HttpGet("{idOption}/Dias/{days}")]
        public IActionResult ListarPorId(int idOption, int days)
        {
            try
            {
                InvestimentoOptionGenerico option = _investimentoOptionsRepository.ListarPorId(idOption);

                if (option == null)
                {
                    return NoContent();
                }

                List<double> indices = _investimentoOptionsRepository.Indices(idOption, days);

                if (option.IdTipo is 1 or 2)
                {
                    return Ok(option);
                }

                var emptyStats = new StatsHistoryOption
                {
                    IdInvestimentoOption = option.IdInvestimentoOption,
                    MarketCap = option.MarketCap,
                };

                var statsProvider = new StatsInvestProvider(_ctx, _memoryCache);
                option.VariacaoPercentual = statsProvider.HistoryOptionStats(emptyStats, 2).VariacaoPeriodoPercentual;

                return Ok(new
                {
                    option,
                    Stats = statsProvider.HistoryOptionStats(emptyStats, days),
                    Emblemas = _investimentoOptionsRepository.ListarEmblemas(idOption, days),
                    Indices = new
                    {
                        MarketCap = indices[0],
                        Dividendos = indices[1],
                        Valorizacao = indices[2],
                        Confiabilidade = indices[3]
                    }
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idTipoOption}/{pagina}/{qntItens}")]
        public IActionResult ListarPorTipoInvestimento(
            byte idTipoOption,
            int pagina,
            int qntItens,
            [FromQuery] short minValorAcao = 0,
            [FromQuery] short maxValorAcao = short.MaxValue,
            [FromQuery] byte minDividendo = 0,
            [FromQuery] long minMarketCap = 0,
            [FromQuery] long maxMarketCap = long.MaxValue,
            [FromQuery] short[] areas = null,
            [FromQuery] string ordenador = ""
            )
        {
            try
            {
                Func<InvestimentoOption, decimal> order = null;
                bool desc = false;

                switch (ordenador)
                {
                    case "marketcapDesc":
                        desc = true;
                        order = o => o.ValorAcao * o.QntCotasTotais;
                        break;
                    case "marketcapAsc":
                        desc = false;
                        order = o => o.ValorAcao * o.QntCotasTotais;
                        break;
                    case "valorDesc":
                        desc = true;
                        order = o => o.ValorAcao;
                        break;
                    case "valorAsc":
                        desc = false;
                        order = o => o.ValorAcao;
                        break;
                    case "dividendoDesc":
                        desc = true;
                        order = o => o.PercentualDividendos;
                        break;
                    default:
                        return BadRequest(new
                        {
                            Message = "Não é possível listar sem um ordenador"
                        });
                }

                if (areas.Length == 0)
                {
                    return Ok(new
                    {
                        optionsList = _investimentoOptionsRepository
                        .AllWhere(
                        o => o.IdAreaInvestimentoNavigation.IdTipoInvestimento == idTipoOption &&
                        o.ValorAcao >= minValorAcao &&
                        o.ValorAcao <= maxValorAcao &&
                        o.PercentualDividendos >= minDividendo,
                        pagina,
                        qntItens,
                        minMarketCap,
                        maxMarketCap,
                        order,
                        desc
                        )
                    });
                }

                return Ok(new
                {
                    optionsList = _investimentoOptionsRepository
                    .AllWhere(
                    o => o.IdAreaInvestimentoNavigation.IdTipoInvestimento == idTipoOption &&
                    o.ValorAcao >= minValorAcao &&
                    o.ValorAcao <= maxValorAcao &&
                    o.PercentualDividendos >= minDividendo &&
                    areas.Contains(o.IdAreaInvestimento),
                    pagina,
                    qntItens,
                    minMarketCap,
                    maxMarketCap,
                    order,
                    desc
                    )
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
            }
        }

        [HttpGet("{pagina}/{qntItens}/{idTipoOption}/comprados/{idUsuario}")]
        public IActionResult ListarJaComprados(int pagina, int qntItens, byte idTipoOption, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                List<InvestimentoOptionMinimo> compradosAnteriormente = _investimentoOptionsRepository.ListarCompradosAnteriormente(pagina, qntItens, idTipoOption, idUsuario);

                return StatusCode(200, new
                {
                    optionList = compradosAnteriormente,
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Favoritos/{idTipoOption}")]
        public IActionResult ListarFavoritos(int[] ids, byte idTipoOption)
        {
            try
            {
                return Ok(new
                {
                    optionsList = _investimentoOptionsRepository.ListarTodosPorId(ids, idTipoOption)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Buscar/{idTipoOption}/{qntItens}")]
        public IActionResult ListarRecomendadas(byte idTipoOption, int qntItens)
        {
            try
            {
                return StatusCode(200, _investimentoOptionsRepository.BuscarInvestimentos(idTipoOption, qntItens).OrderBy(p => p.Nome));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateFicOption()
        {
            try
            {
                return StatusCode(201, _investimentoOptionsRepository.CreateFicOption());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPut("{idOption}")]
        public IActionResult Atualizar(short idOption, InvestimentoOption updatedOption)
        {
            try
            {
                _investimentoOptionsRepository.Update(idOption, updatedOption);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
