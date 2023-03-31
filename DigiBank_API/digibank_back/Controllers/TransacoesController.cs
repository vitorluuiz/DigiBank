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
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoRepository _transacoesRepository;
        public TransacoesController()
        {
            _transacoesRepository = new TransacaoRepository();
        }

        [HttpGet("{pagina}/{qntItens}")]
        public IActionResult ListarTodas(int pagina, int qntItens)
        {
            try
            {
                return Ok(_transacoesRepository.ListarTodas(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("ListarPorId/{idTransacao}")]
        public IActionResult ListarPorId(int idTransacao)
        {
            try
            {
                return StatusCode(200, _transacoesRepository.ListarPorid(idTransacao));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Recebidas/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult ListarRecebidas(int idUsuario, int pagina, int qntItens)
        {
            try
            {
                return Ok(_transacoesRepository.ListarRecebidas(idUsuario, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Enviadas/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult ListarEnviadas(int idUsuario, int pagina, int qntItens)
        {
            try
            {
                return Ok(_transacoesRepository.ListarEnviadas(idUsuario, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("FluxoTotalEntreUsuarios")]
        public IActionResult FluxoTotal(int idPagante, int idRecebente)
        {
            try
            {
                return Ok(_transacoesRepository.FluxoTotal(idPagante, idRecebente));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("EntreUsuarios/{pagina}/{qntItens}")]
        public IActionResult ListarEntreUsuarios(int idUsuario1, int idUsuario2, int pagina, int qntItens)
        {
            try
            {
                return Ok(_transacoesRepository.ListarEntreUsuarios(idUsuario1, idUsuario2, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("EfetuarTransacao")]
        public IActionResult Cadastrar(Transaco newTransacao)
        {
            try
            {
                bool isSucess = _transacoesRepository.EfetuarTransacao(newTransacao);
                if(isSucess)
                {
                return StatusCode(201);
                }
                return BadRequest("Saldo insuficiente");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("Id/{idTransacao}")]
        public IActionResult Deletar(int idTransacao)
        {
            try
            {
                _transacoesRepository.Deletar(idTransacao);
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
