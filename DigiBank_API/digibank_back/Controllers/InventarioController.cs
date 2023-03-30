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
    public class InventarioController : ControllerBase
    {
        private readonly IInventarioRepository _inventarioRepository;
        public InventarioController()
        {
            _inventarioRepository = new InventarioRepository();
        }

        [HttpGet("MeuInventario/{idUsuario}")]
        public IActionResult BuscarMeuInventario(int idUsuario)
        {
            try
            {
                return Ok(_inventarioRepository.ListarMeuInventario(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("IdItem/{idItem}")]
        public IActionResult BuscarPorId(int idItem)
        {
            try
            {
                return Ok(_inventarioRepository.ListarPorId(idItem));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

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

        [HttpPatch("Mover")]
        public IActionResult Mover(int idItem, int idDestino)
        {
            try
            {
                bool isSucess = _inventarioRepository.Mover(idItem, idDestino);

                if (isSucess)
                {
                    return Ok("Transferência concluida");
                }

                return BadRequest("Transferência não efetuada");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("Id/{idItem}")]
        public IActionResult Deletar(int idItem)
        {
            try
            {
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
