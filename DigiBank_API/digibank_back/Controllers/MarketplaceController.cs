using Microsoft.AspNetCore.Http;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using digibank_back.Utils;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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

        [HttpGet("{pagina}/{qntItens}")]
        public IActionResult ListarTodos(int pagina, int qntItens)
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarTodos(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpGet("Privados")]
        public IActionResult ListarPrivados() 
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarInativos());
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
                string[] extensoesPermitidas = { "jpg", "png", "jpeg"};
                
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
        public IActionResult Comprar(int idComprador, int idPost, [FromHeader] string Authorization)
        {
            try
            {
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idComprador);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

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
        public IActionResult DeixarPrivado(int idPost, [FromHeader] string Authorization)
        {
            try
            {
                Marketplace post = _marketplaceRepository.ListarPorId(idPost);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (isAcessful)
                {
                    _marketplaceRepository.TurnInative(idPost);

                    return Ok();
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

        [HttpPatch("Desprivar/{idPost}")]
        public IActionResult DeixarPublico(int idPost, [FromHeader] string Authorization)
        {
            try
            {
                Marketplace post = _marketplaceRepository.ListarPorId(idPost);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (isAcessful)
                {
                    _marketplaceRepository.TurnActive(idPost);

                    return Ok();
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


        [HttpDelete("Id/{idPost}")]
        public IActionResult Remover(int idPost, [FromHeader] string Authorization)
        {
            try
            {
                Marketplace post = _marketplaceRepository.ListarPorId(idPost);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (isAcessful)
                {
                    _marketplaceRepository.Deletar(idPost);

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
