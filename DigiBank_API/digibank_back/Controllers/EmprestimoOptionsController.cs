using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmprestimoOptionsController : ControllerBase
    {
        private readonly IEmprestimosOptionsRepository _emprestimosOptionsRepository;
        public EmprestimoOptionsController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _emprestimosOptionsRepository = new EmprestimosOptionsRepository(ctx, memoryCache);
        }

        [HttpGet("{pagina}/{qntItens}")]
        public IActionResult GetEmprestimosOptions(int pagina, int qntItens)
        {
            try
            {
                return Ok(_emprestimosOptionsRepository.ListarTodos(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult GetEmprestimosOptions(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_emprestimosOptionsRepository.ListarDisponiveis(idUsuario, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idEmprestimoOption}")]
        public IActionResult ListarPorId(int idEmprestimoOption)
        {
            try
            {
                return Ok(_emprestimosOptionsRepository.ListarPorId(idEmprestimoOption));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Prever/{idEmprestimoOption}")]
        public IActionResult Prever(int idEmprestimoOption)
        {
            try
            {
                EmprestimosOption emprestimo = _emprestimosOptionsRepository.ListarPorId(idEmprestimoOption);

                return Ok(_emprestimosOptionsRepository.CalcularPrevisao(emprestimo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPost]
        public IActionResult Cadastrar(EmprestimosOption newOption)
        {
            try
            {
                _emprestimosOptionsRepository.Cadastrar(newOption);

                return StatusCode(201);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPut("{idEmprestimoOption}")]
        public IActionResult Atualizar(int idEmprestimoOption, EmprestimosOption optionAtualizada)
        {
            try
            {
                _emprestimosOptionsRepository.Atualizar(idEmprestimoOption, optionAtualizada);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpDelete("{idEmprestimoOption}")]
        public IActionResult Deletar(int idEmprestimoOption)
        {
            try
            {
                _emprestimosOptionsRepository.Deletar(idEmprestimoOption);

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
