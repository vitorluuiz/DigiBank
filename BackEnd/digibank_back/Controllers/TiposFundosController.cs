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
    public class TiposFundosController : ControllerBase
    {
        private readonly ITipoFundoRepository _tipoFundoRepository;

        public TiposFundosController()
        {
            _tipoFundoRepository = new TipoFundoRepository();
        }

        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return StatusCode(200, _tipoFundoRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(TiposFundo newFundo)
        {
            try
            {
                _tipoFundoRepository.Cadastrar(newFundo);
                return StatusCode(201);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("id/{idTipoFundo}")]
        public IActionResult Deletar(int idTipoFundo)
        {
            try
            {
                _tipoFundoRepository.Deletar(idTipoFundo);
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
