using digibank_back.Domains;
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
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
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
                Inventario item = _inventarioRepository.ListarPorId(idItem);

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, item.IdUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

                if(item != null)
                {
                    return Ok(item);
                }

                return NotFound(new
                {
                    Message = "Item não existe"
                });
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
                Inventario item = _inventarioRepository.ListarPorId(idItem);

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, item.IdUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
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
                Inventario item = _inventarioRepository.ListarPorId(idItem);

                if(item == null)
                {
                    return NotFound(new
                    {
                        Message = "Item não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, item.IdUsuario);

                if(isAcessful)
                {
                    _inventarioRepository.Deletar(idItem);

                    return StatusCode(204);
                }

                return StatusCode(403, new
                {
                    Message = "Sem acesso"
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
