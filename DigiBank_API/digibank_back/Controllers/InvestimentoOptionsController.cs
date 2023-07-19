using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class InvestimentoOptionsController : ControllerBase
    {
        private readonly IInvestimentoOptionsRepository _investimentoOptionsRepository;
        public InvestimentoOptionsController()
        {
            _investimentoOptionsRepository = new InvestimentoOptionsRepository();
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

        [HttpGet("{idInvestimentoOption}/Dias/{days}")]
        public IActionResult ListarPorId(int idInvestimentoOption, int days)
        {
            try
            {
                InvestimentoOption option = _investimentoOptionsRepository.ListarPorId(idInvestimentoOption);

                if (option == null)
                {
                    return NoContent();
                }

                List<double> indices = _investimentoOptionsRepository.ListarIndices(idInvestimentoOption, days);

                return Ok(new
                {
                    option,
                    Stats = _investimentoOptionsRepository.ListarStatsHistoryOption(idInvestimentoOption, days),
                    Emblemas = _investimentoOptionsRepository.ListarEmblemas(idInvestimentoOption, days),
                    Indices = new
                    {
                        ValorAcao = indices[0],
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

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult CreateFicOption()
        {
            try
            {
                _investimentoOptionsRepository.CreateFicOption();
                return StatusCode(201);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPut("{idInvestimentoOption}")]
        public IActionResult Atualizar(int idInvestimentoOption, InvestimentoOption updatedOption)
        {
            try
            {
                _investimentoOptionsRepository.Atualizar(idInvestimentoOption, updatedOption);

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
