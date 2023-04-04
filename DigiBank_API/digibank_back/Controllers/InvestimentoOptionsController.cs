using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpGet("{idInvestimentoOption}")]
        public IActionResult ListarPorId(int idInvestimentoOption)
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

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Cadastrar(InvestimentoOption newOption)
        {
            try
            {
                _investimentoOptionsRepository.Cadastrar(newOption);
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

        [Authorize(Roles = "1")]
        [HttpDelete("{idInvestimentoOption}")]
        public IActionResult Deletar(int idInvestimentoOption)
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
