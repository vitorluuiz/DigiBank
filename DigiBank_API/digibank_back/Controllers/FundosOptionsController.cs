using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FundosOptionsController : ControllerBase
    {
        private readonly IFundosOptionsRepository _fundosOptionsRepository;
        public FundosOptionsController()
        {
            _fundosOptionsRepository= new FundosOptionsRepository();
        }

        [HttpGet]
        public IActionResult ListarOptions()
        {
            try
            {
                return Ok(_fundosOptionsRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Id/{idFundoOption}")]
        public IActionResult ListarPorId(int idFundoOption)
        {
            try
            {
                return Ok(_fundosOptionsRepository.ListarPorId(idFundoOption));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(FundosOption newOption)
        {
            try
            {
                _fundosOptionsRepository.Cadastrar(newOption);
                return StatusCode(201);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPut("Id/{idFundoOption}")]
        public IActionResult Atualizar(int idFundoOption, FundosOption updatedOption)
        {
            try
            {
                _fundosOptionsRepository.Atualizar(idFundoOption, updatedOption);
                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("Id/{idFundoOption}")]
        public IActionResult Deletar(int idFundoOption)
        {
            try
            {
                _fundosOptionsRepository.Deletar(idFundoOption);
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
