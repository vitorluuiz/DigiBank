using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using digibank_back.Contexts;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioRepository _inventarioRepository;
        public InventarioController(digiBankContext ctx)
        {
            _inventarioRepository = new InventarioRepository(ctx);
        }

        [HttpGet("MeuInventario/{idUsuario}")]
        public IActionResult BuscarMeuInventario(int idUsuario, [FromHeader] string Authorization,
            [FromQuery] int pagina = 1,
            [FromQuery] int qntItens = 10)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_inventarioRepository.Meu(idUsuario, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("{idItem}")]
        public IActionResult Deletar(int idItem, [FromHeader] string Authorization)
        {
            try
            {
                InventarioUser item = _inventarioRepository.PorId(idItem);

                if (item == null)
                {
                    return NotFound(new
                    {
                        Message = "Item não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, item.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _inventarioRepository.Deletar(idItem);

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
