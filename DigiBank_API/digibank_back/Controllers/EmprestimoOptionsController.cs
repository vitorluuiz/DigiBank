using digibank_back.Domains;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmprestimoOptionsController : ControllerBase
    {
        private readonly IEmprestimosOptionsRepository _emprestimosOptionsRepository;
        public EmprestimoOptionsController()
        {
            _emprestimosOptionsRepository = new EmprestimosOptionsRepository();
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

        [HttpGet("Id/{idEmprestimoOption}")]
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

        [HttpGet("Prever/IdOption/{idEmprestimoOption}")]
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

        [HttpPut("Id/{idEmprestimoOption}")]
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

        [HttpDelete("Id/{idEmprestimoOption}")]
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
