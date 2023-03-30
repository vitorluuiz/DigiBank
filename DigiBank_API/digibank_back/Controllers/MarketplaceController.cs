using Microsoft.AspNetCore.Http;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using digibank_back.Utils;
using Microsoft.Extensions.Hosting;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class MarketplaceController : Controller
    {
        private readonly IMarketplaceRepository _marketplaceRepository;
        public MarketplaceController()
        {
            _marketplaceRepository = new MarketplaceRepository();   
        }

        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Privados")]
        public IActionResult ListarPrivados() 
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarInvisibles());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("ListarPorId/{idPost}")]
        public IActionResult ListarPorId(int idPost)
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarPorId(idPost));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] Marketplace newPost, List<IFormFile> imgsPost, IFormFile imgPrincipal)
        {
            try
            {
                string[] extensoesPermitidas = { "jpg", "png", "jpeg", "gif" };
                
                string uploadResultados = Upload.UploadFile(imgPrincipal, extensoesPermitidas);

                if (uploadResultados == "Sem arquivo")
                {
                    return StatusCode(400, "Não é possível cadastar um produto sem ao menos uma imagem");
                }
                if (uploadResultados == "Extenção não permitida")
                {
                    return BadRequest("Extensão de arquivo não permitida");
                }

                newPost.MainImg = uploadResultados;

                Marketplace post = _marketplaceRepository.Cadastrar(newPost);

                if (imgsPost.Count > 0)
                {
                    Upload.UploadFiles(imgsPost, extensoesPermitidas, post.IdPost);
                }

                return StatusCode(201, post);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Comprar")]
        public IActionResult Comprar(int idComprador, int idPost)
        {
            try
            {
                bool isSucess = _marketplaceRepository.Comprar(idComprador, idPost);

                if(isSucess)
                {
                    return Ok("Compra efetuada");
                }

                return BadRequest("Não foi possível realizar a compra");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("Privar/{idPost}")]
        public IActionResult DeixarPrivado(int idPost)
        {
            try
            {
                _marketplaceRepository.TurnInvisible(idPost);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("Desprivar/{idPost}")]
        public IActionResult DeixarPublico(int idPost)
        {
            try
            {
                _marketplaceRepository.TurnVisible(idPost);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }


        [HttpDelete("Id/{idPost}")]
        public IActionResult Remover(int idPost)
        {
            try
            {
                _marketplaceRepository.Deletar(idPost);
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
