using digibank_back.Contexts;
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
    public class AreaInvestimentoController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;
        public AreaInvestimentoController(digiBankContext ctx)
        {
            _areaRepository = new AreaRepository(ctx);
        }

        [HttpGet("Tipo/{idTipo}")]
        public IActionResult GetByTipo(int idTipo)
        {
            try
            {
                return Ok(_areaRepository.GetByTipo(idTipo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
