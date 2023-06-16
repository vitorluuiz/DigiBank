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
using digibank_back.DTOs;
using System.Linq;
using digibank_back.ViewModel;

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

        [HttpGet("{pagina}/{qntItens}/vendas")]
        public IActionResult ListarPorVendas(int pagina, int qntItens)
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarTodos(pagina, qntItens).OrderByDescending(i => i.Vendas));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{pagina}/{qntItens}/avaliacao")]
        public IActionResult ListarPorAvaliacao(int pagina, int qntItens)
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarTodos(pagina, qntItens).OrderByDescending(i => i.Avaliacao));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Recomendadas/{qntItens}")]
        public IActionResult ListarRecomendadas(int qntItens)
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.SearchBestResults(qntItens).OrderBy(p => p.Titulo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Usuario/{idUsuario}")]
        public IActionResult ListarDeUsuario(int idUsuario)
        {
            try
            {
                return StatusCode(200, _marketplaceRepository.ListarDeUsuario(idUsuario));
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

        [HttpGet("{idPost}")]
        public IActionResult ListarPorId(int idPost, [FromHeader] string Authorization)
        {
            try
            {
                PostGenerico post = _marketplaceRepository.ListarPorIdPublico(idPost, true);

                if(post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                return Ok(post);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar([FromForm] MarketPlaceViewModel newPost, List<IFormFile> imgsPost, IFormFile imgPrincipal, [FromHeader] string Authorization)
        {
            try
            {
                if(newPost == null || newPost.IdUsuario == 0)
                {
                    return BadRequest(new
                    {
                        Message = "Impossível cadastrar um Post vazio"
                    });
                }

                Marketplace post = new Marketplace
                {
                    IdUsuario = (short)newPost.IdUsuario,
                    Nome = newPost.Titulo,
                    Descricao = newPost.Descricao,
                    Valor = newPost.Valor,
                    MainColorHex = newPost.MainColorHex,
                    IsVirtual = newPost.IsVirtual,
                };

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                string[] extensoesPermitidas = { "jpg", "png", "jpeg", "svg"};
                
                string uploadResultados = Upload.UploadFile(imgPrincipal, extensoesPermitidas);

                if (uploadResultados == "Sem arquivo")
                {
                    return StatusCode(400, "Não é possível cadastar um produto sem ao menos uma imagem");
                }
                else if (uploadResultados == "Extensão não permitida")
                {
                    return BadRequest("Extensão de arquivo não permitida");
                }

                post.MainImg = uploadResultados;

                Marketplace postCadastrado = _marketplaceRepository.Cadastrar(post);

                string errorImgs = null;
                if (imgsPost.Count > 0)
                {
                    errorImgs = Upload.UploadFiles(imgsPost, extensoesPermitidas, post.IdPost);
                }

                return StatusCode(201, new
                {
                    PostData = post,
                    ImgsErrors = errorImgs
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Comprar/{idPost}/{idComprador}")]
        public IActionResult Comprar(int idComprador, int idPost, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idComprador);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucess = _marketplaceRepository.Comprar(idComprador, idPost);

                if(isSucess)
                {
                    return Ok(new
                    {
                        Message = "Compra realizada"
                    });
                }

                return BadRequest(new
                {
                    Message = "Compra não concluída"
                });
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
                Marketplace post = _marketplaceRepository.ListarPorId(idPost, true);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, post.IdPost);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _marketplaceRepository.TurnInative(idPost);

                return Ok();
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
                Marketplace post = _marketplaceRepository.ListarPorId(idPost, true);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _marketplaceRepository.TurnActive(idPost);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }


        [HttpDelete("{idPost}")]
        public IActionResult Remover(int idPost, [FromHeader] string Authorization)
        {
            try
            {
                Marketplace post = _marketplaceRepository.ListarPorId(idPost, true);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

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
