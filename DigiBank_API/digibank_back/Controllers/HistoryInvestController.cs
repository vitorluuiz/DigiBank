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
    public class HistoryInvestController : ControllerBase
    {
        readonly IHistoryInvestRepository _historyInvestRepository;
        public HistoryInvestController()
        {
            _historyInvestRepository = new HistoryInvestRepository();
        }

        [HttpGet("Investimento/Saldo/{idUsuario}/{days}")]
        public IActionResult GetInvestHistory(int idUsuario, int days)
        {
            try
            {
                return Ok(new
                {
                    historyList = _historyInvestRepository.GetHistoryFromInvest(idUsuario, DateTime.Now.AddDays(-days), DateTime.Now)
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
                    historyList = _historyInvestRepository.GetHistoryFromOption(idOption, days)
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
