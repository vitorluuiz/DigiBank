using digibank_back.Contexts;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Linq;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryInvestController : ControllerBase
    {
        readonly IHistoryInvestRepository _historyInvestRepository;
        public HistoryInvestController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _historyInvestRepository = new HistoryInvestRepository(ctx, memoryCache);
        }

        [HttpGet("Investimento/Saldo/{idUsuario}/{months}")]
        public IActionResult GetInvestHistory(int idUsuario, int months, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(new
                {
                    historyList = _historyInvestRepository.GetHistoryFromInvest(idUsuario, DateTime.Now.AddMonths(-months), DateTime.Now)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
        [HttpGet("Investimento/Saldo/Poupanca/{idUsuario}/{days}")]
        public IActionResult GetInvestHistoryPoupanca(int idUsuario, int days, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(new
                {
                    historyList = _historyInvestRepository.GetHistoryFromPoupanca(idUsuario, DateTime.Now.AddDays(-days), DateTime.Now)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Option/{idOption}/{days}")]
        public IActionResult GetByHistoryOption(int idOption, int days)
        {
            try
            {
                return Ok(new
                {
                    historyList = _historyInvestRepository.GetHistoryFromOption(idOption, days).OrderBy(h => h.DataH)
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
