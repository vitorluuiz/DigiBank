using digibank_back.Contexts;
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

        [HttpGet("{pagina}/{qntItens}")]
        public IActionResult ListarOptions(int pagina, int qntItens)
        {
            try
            {
                return Ok(_investimentoOptionsRepository.ListarTodos(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
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
                    IdInvestimentoOption = option.IdOption,
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
        public IActionResult ListarPorTipoInvestimento(byte idTipoOption, int pagina, int qntItens)
        {
            try
            {
                return Ok(_investimentoOptionsRepository.ListarPorTipoInvestimento(idTipoOption, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
            }
        }
        [HttpGet("{pagina}/{qntItens}/{idTipoOption}/vendas")]
        public IActionResult ListarPorVendas(byte idTipoOption, int pagina, int qntItens)
        {
            try
            {
                return StatusCode(200, _investimentoOptionsRepository.ListarPorTipoInvestimento(idTipoOption, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
        [HttpGet("{pagina}/{qntItens}/{idTipoOption}/valor/{valorMax}")]
        public IActionResult ListarPorValorMax(byte idTipoOption, int pagina, int qntItens, int valorMax)
        {
            try
            {
                List<InvestimentoOptionMinimo> investimentos = _investimentoOptionsRepository.ListarPorTipoInvestimento(idTipoOption, pagina, qntItens);

                if (valorMax == -1)
                {
                    return StatusCode(200, investimentos.OrderByDescending(o => o.Valor));
                }

                return StatusCode(200, investimentos.Where(o => o.Valor <= valorMax).OrderByDescending(o => o.Valor));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
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

                int optionCount = compradosAnteriormente.Count;

                return StatusCode(200, new
                {
                    optionList = compradosAnteriormente,
                    optionCount
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Favoritos")]
        public IActionResult ListarFavoritos(int[] ids)
        {
            try
            {
                return Ok(new
                {
                    optionsList = _investimentoOptionsRepository.ListarTodosPorId(ids)
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
