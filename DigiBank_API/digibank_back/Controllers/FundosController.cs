using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FundosController : ControllerBase
    {
        private readonly IFundoRepository _fundoRepository;
        public FundosController()
        {
            _fundoRepository = new FundoRepository();
        }

        [HttpGet]
        public IActionResult ListarFundos()
        {
            try
            {
                return Ok(_fundoRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Id/{idFundo}")]
        public IActionResult ListarPorId(int idFundo) 
        {
            try
            {
                return Ok(_fundoRepository.ListarPorId(idFundo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("IdUsuario/{idUsuario}")]
        public IActionResult ListarDeUsuario(int idUsuario) 
        {
            try
            {
                return Ok(_fundoRepository.ListarDeUsuario(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("PreverGanhos")]
        public IActionResult Prever(int idFundo, int diasInvestidos)
        {
            try
            {
                return Ok(_fundoRepository.CalcularPrevisao(idFundo, diasInvestidos));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Comprar")]
        public IActionResult Comprar(Fundo newFundo, int idUsuario)
        {
            try
            {
                newFundo.DataInicio = DateTime.Now;

                bool isSucess = _fundoRepository.Comprar(newFundo, idUsuario);

                if (isSucess)
                {
                    return Ok();
                }

                return BadRequest("Não foi possível efetuar a compra");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Vender")]
        public IActionResult Vender(int idFundo, int idVendedor)
        {
            try
            {
                _fundoRepository.Vender(idFundo, idVendedor);

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
