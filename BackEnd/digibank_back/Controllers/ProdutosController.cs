using Microsoft.AspNetCore.Http;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class ProdutosController : Controller
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutosController()
        {
            _produtoRepository = new ProdutoRepository();   
        }

        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return StatusCode(200, _produtoRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("ListarPorId/{idProduto}")]
        public IActionResult ListarPorId(int idProduto)
        {
            try
            {
                return StatusCode(200, _produtoRepository.ListarPorId(idProduto));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(Produto newProduto)
        {
            try
            {
                return StatusCode(201, _produtoRepository.Cadastrar(newProduto));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("Id/{idProduto}")]
        public IActionResult Remover(int idProduto)
        {
            try
            {
                _produtoRepository.Deletar(idProduto);
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
