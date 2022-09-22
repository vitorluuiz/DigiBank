using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]

    [Route("api/[controller]")]

    [ApiController]
    public class ProdutosController : Controller
    {
        private IProdutoRepository _produtoRepository;
        ProdutosController()
        {
            _produtoRepository = new ProdutoRepository();   
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
                return BadRequest(error) ;
                throw;
            }
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
    }
}
