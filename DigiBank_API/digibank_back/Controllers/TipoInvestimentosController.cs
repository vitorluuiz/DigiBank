using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class TipoInvestimentosController : ControllerBase
    {
        private readonly ITipoInvestimentoRepository _tipoInvestimentoRepository;

        public TipoInvestimentosController()
        {
            _tipoInvestimentoRepository = new TipoInvestimentoRepository();
        }

        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return StatusCode(200, _tipoInvestimentoRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(TipoInvestimento newInvestimento)
        {
            try
            {
                _tipoInvestimentoRepository.Cadastrar(newInvestimento);

                return StatusCode(201);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("id/{idTipoInvestimento}")]
        public IActionResult Deletar(int idTipoInvestimento)
        {
            try
            {
                _tipoInvestimentoRepository.Deletar(idTipoInvestimento);

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
