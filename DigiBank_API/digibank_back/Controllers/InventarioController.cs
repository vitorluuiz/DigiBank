using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Net;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioRepository _inventarioRepository;
        public InventarioController()
        {
            _inventarioRepository = new InventarioRepository();
        }

        [HttpGet("MeuInventario/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult BuscarMeuInventario(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_inventarioRepository.ListarMeuInventario(idUsuario, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idItem}")]
        public IActionResult BuscarPorId(int idItem, [FromHeader] string Authorization)
        {
            try
            {
                InventarioUser item = _inventarioRepository.ListarPorId(idItem);

                Console.WriteLine(item);

                if (item == null)
                {
                    return BadRequest(new
                    {
                        message = "Item não encontrado"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, item.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                if(item == null)
                {
                    return NotFound(new
                    {
                        Message = "Item não existe"
                    });
                }
                
                return Ok(item);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Depositar(Inventario newItem)
        {
            try
            {
                _inventarioRepository.Depositar(newItem);

                return StatusCode(201);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("Mover/{idItem}/{idDestino}")]
        public IActionResult Mover(int idItem, int idDestino, [FromHeader] string Authorization)
        {
            try
            {
                InventarioUser item = _inventarioRepository.ListarPorId(idItem);

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, item.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucess = _inventarioRepository.Mover(idItem, idDestino);

                if (isSucess)
                {
                    return Ok(new
                    {
                        Message = "Transferência concluida"
                    });
                }

                return BadRequest(new
                {
                    Message = "Trasferência não efetuada"
                });
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
                InventarioUser item = _inventarioRepository.ListarPorId(idItem);

                if(item == null)
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
