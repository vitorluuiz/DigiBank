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
    public class CondicoesController : ControllerBase
    {
        private readonly ICondicaoRepository _condicaoRepository;
        public CondicoesController()
        {
            _condicaoRepository = new CondicaoRepository();
        }

        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return StatusCode(200, _condicaoRepository.ListarTodas());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(Condico newCondicao)
        {
            try
            {
                return StatusCode(201, _condicaoRepository.Cadastrar(newCondicao));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("/Deletar/{idCondicao}")]
        public IActionResult Deletar(int idCondicao)
        {
            try
            {
                _condicaoRepository.Deletar(idCondicao);
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
