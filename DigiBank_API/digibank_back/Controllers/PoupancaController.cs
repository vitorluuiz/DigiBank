using digibank_back.DTOs;
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
    public class PoupancaController : ControllerBase
    {
        readonly IPoupancaRepository _poupancaRepository;
        public PoupancaController()
        {
            _poupancaRepository = new PoupancaRepository();
        }

        [HttpGet("{idUsuario}")]
        public IActionResult GetPoupanca(int idUsuario)
        {
            try
            {
                return Ok(new Poupanca(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Depositar")]
        public IActionResult Depositar(int idUsuario, decimal quantidade)
        {
            try
            {
                if (_poupancaRepository.Depositar(idUsuario, quantidade))
                {
                    return Ok(new
                    {
                        Message = "Depósito aprovado"
                    });
                }

                return BadRequest(new
                {
                    Error = "Depósito não aprovado"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);

                throw;
            }
        }

        [HttpPost("Ganhos")]
        public IActionResult CalcularGanhos(int idUsuario, DateTime inicio, DateTime fim)
        {
            try
            {
                return Ok(new
                {
                    Ganhos = new
                    {
                        Inicio = inicio,
                        Fim = fim,
                        Ganhos = _poupancaRepository.CalcularGanhos(idUsuario, inicio, fim)
                    }
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
