using Microsoft.AspNetCore.Http;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using digibank_back.Utils;

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
        public IActionResult Cadastrar([FromForm] Produto newProduto, [FromForm] List<IFormFile> imgsProduto)
        {
            try
            {
                Produto produto = _produtoRepository.Cadastrar(newProduto);
                string[] extensoesPermitidas = { "jpg", "png", "jpeg", "gif" };
                
                if(imgsProduto.Count > 1)
                {
                    Upload.UploadFile(imgsProduto, extensoesPermitidas, newProduto.IdProduto);
                }

                string uploadResultados = Upload.UploadFile(imgsProduto[0], extensoesPermitidas);

                if (uploadResultados == "Sem arquivo")
                {
                    return StatusCode(400, "Não é possível cadastar um produto sem ao menos uma imagem");
                }
                if (uploadResultados == "Extenção não permitida")
                {
                    return BadRequest("Extensão de arquivo não permitida");
                }

                return StatusCode(201, produto);
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
