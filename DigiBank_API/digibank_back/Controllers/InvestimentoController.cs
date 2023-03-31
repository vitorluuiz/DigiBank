using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class InvestimentoController : ControllerBase
    {
        private readonly IInvestimentoRepository _investimentoRepository;
        public InvestimentoController()
        {
            _investimentoRepository = new InvestimentoRepository();
        }

        [HttpGet]
        public IActionResult ListarFundos()
        {
            try
            {
                return Ok(_investimentoRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Id/{idInvestimento}")]
        public IActionResult ListarPorId(int idInvestimento) 
        {
            try
            {
                return Ok(_investimentoRepository.ListarPorId(idInvestimento));
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
                return Ok(_investimentoRepository.ListarDeUsuario(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("PreverGanhos/{idInvestimento}/{diasInvestidos}")]
        public IActionResult Prever(int idInvestimento, int diasInvestidos)
        {
            try
            {
                return Ok(_investimentoRepository.CalcularPrevisao(idInvestimento, diasInvestidos));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("CalcularGanhos/{idInvestimento}")]
        public IActionResult CalcularGanhos(int idInvestimento)
        {
            try
            {
                return Ok(_investimentoRepository.CalcularGanhos(idInvestimento));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Comprar")]
        public IActionResult Comprar(Investimento newInvestimento, int idUsuario)
        {
            try
            {
                newInvestimento.DataAquisicao = DateTime.Now;

                bool isSucess = _investimentoRepository.Comprar(newInvestimento, idUsuario);

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
        public IActionResult Vender(int idInvestimento, int idVendedor)
        {
            try
            {
                _investimentoRepository.Vender(idInvestimento, idVendedor);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("VenderCotas")]
        public IActionResult VenderCotas(int idInvestimento, decimal qntCotas)
        {
            try
            {
                _investimentoRepository.VenderCotas(idInvestimento, qntCotas);

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
