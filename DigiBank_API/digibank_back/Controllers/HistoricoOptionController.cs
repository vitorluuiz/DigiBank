using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class HistoricoOptionController : ControllerBase
    {
        readonly IHistoricoOptionsRepository _historyOptionsRepository;
        public HistoricoOptionController()
        {
            _historyOptionsRepository = new HistoricoOptionsRepository();
        }

        [HttpGet("{idOption}/{pagina}/{qntItens}")]
        public IActionResult GetByHistoryOption(int idOption, int pagina, int qntItens)
        {
            try
            {
                return Ok(new
                {
                    historyList = _historyOptionsRepository.GetHistoryFromOption(idOption, pagina, qntItens).OrderByDescending(H => H.DataHistorico)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult GerarOptionHistory(int idOption)
        {
            try
            {
                _historyOptionsRepository.UpdateHistory(idOption);

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
