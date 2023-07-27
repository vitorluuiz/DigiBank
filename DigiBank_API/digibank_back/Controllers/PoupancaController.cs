using digibank_back.Contexts;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.ViewModel.DataTimeInterval;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PoupancaController : ControllerBase
    {
        readonly IPoupancaRepository _poupancaRepository;
        readonly IMemoryCache _memoryCache;
        public PoupancaController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _poupancaRepository = new PoupancaRepository(ctx, memoryCache);
            _memoryCache = memoryCache;
        }
        [HttpGet("{idUsuario}")]
        public IActionResult GetPoupanca(int idUsuario)
        {
            try
            {
                DateTime today = DateTime.Now;
                return Ok(new Poupanca(idUsuario, new digiBankContext(), _memoryCache)
                {
                    GanhoDiario = _poupancaRepository.CalcularLucro(idUsuario, today.AddDays(-1), today),
                    GanhoMensal = _poupancaRepository.CalcularLucro(idUsuario, today.AddMonths(-1), today),
                    GanhoAnual = _poupancaRepository.CalcularLucro(idUsuario, today.AddYears(-1), today)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Depositar/{idUsuario}/{quantidade}")]
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

        [HttpPost("Sacar/{idUsuario}/{quantidade}")]
        public IActionResult Sacar(int idUsuario, decimal quantidade)
        {
            try
            {
                if (_poupancaRepository.Sacar(idUsuario, quantidade))
                {
                    return Ok(new
                    {
                        Message = "Saque aprovado"
                    });
                }

                return BadRequest(new
                {
                    Error = "Saque não aprovado"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);

                throw;
            }
        }

        [HttpPost("Ganhos/{idUsuario}")]
        public IActionResult CalcularGanhos(int idUsuario, DataInterval Intervalo)
        {
            try
            {
                return Ok(new
                {
                    Ganhos = new
                    {
                        Inicio = Intervalo.Inicio,
                        Fim = Intervalo.Fim,
                        Ganhos = _poupancaRepository.CalcularLucro(idUsuario, Intervalo.Inicio, Intervalo.Fim)
                    }
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Saldo")]
        public IActionResult CalcularSaldo(int idUsuario, DateTime data)
        {
            try
            {
                return Ok(new
                {
                    Saldo = _poupancaRepository.Saldo(idUsuario, data)
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
