using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("{idInvestimentoOption}")]
        public IActionResult ListarPorId(short idInvestimentoOption)
        {
            try
            {
                return Ok(_investimentoOptionsRepository.ListarPorId(idInvestimentoOption));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
        [HttpGet("{idTipoInvestimento}/{pagina}/{qntItens}")]
        public IActionResult ListarPorTipoInvestimento(byte idTipoInvestimentoOption, int pagina, int qntItens)
        {
            try
            {
                return Ok(_investimentoOptionsRepository.ListarPorTipoInvestimento(idTipoInvestimentoOption, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
            }
        }
        [HttpGet("{pagina}/{qntItens}/vendas")]
        public IActionResult ListarPorVendas(int pagina, int qntItens)
        {
            try
            {
                return StatusCode(200, _investimentoOptionsRepository.ListarTodos(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
        [HttpGet("{pagina}/{qntItens}/valor/{valorMax}")]
        public IActionResult ListarPorValorMax(int pagina, int qntItens, int valorMax)
        {
            try
            {
                List<InvestimentoOptionGenerico> investimentos = _investimentoOptionsRepository.ListarTodos(pagina, qntItens);

                if (valorMax == -1)
                {
                    return StatusCode(200, investimentos.OrderByDescending(o => o.ValorAcao));
                }

                return StatusCode(200, investimentos.Where(o => o.ValorAcao <= valorMax).OrderByDescending(o => o.ValorAcao));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
        [HttpGet("{pagina}/{qntItens}/comprados/{idUsuario}")]
        public IActionResult ListarJaComprados(int pagina, int qntItens, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return StatusCode(200, _investimentoOptionsRepository.ListarCompradosAnteriormente(pagina, qntItens, idUsuario));
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
        public IActionResult Atualizar(short idInvestimentoOption, InvestimentoOption updatedOption)
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

        [Authorize(Roles = "1")]
        [HttpDelete("{idInvestimentoOption}")]
        public IActionResult Deletar(short idInvestimentoOption)
        {
            try
            {
                _investimentoOptionsRepository.Deletar(idInvestimentoOption);

                return StatusCode(204);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }


    }
}
