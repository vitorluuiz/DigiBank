using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class MarketplaceController : Controller
    {
        private readonly IMarketplaceRepository _marketplaceRepository;
        public MarketplaceController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _marketplaceRepository = new MarketplaceRepository(ctx, memoryCache);
        }

        [HttpGet("{pagina}/{qntItens}/{ordenador}")]
        public IActionResult ListarOrdenado(int pagina, int qntItens, string ordenador)
        {
            try
            {
                switch (ordenador)
                {
                    case "vendas":
                        return StatusCode(200, _marketplaceRepository.AllOrderBy(p => p.Vendas, pagina, qntItens, true));
                    case "avaliacao":
                        return StatusCode(200, _marketplaceRepository.AllOrderBy(p => (decimal)p.Avaliacao, pagina, qntItens, true));
                    default:
                        return BadRequest(new
                        {
                            Message = "Ordenação desconhecida"
                        });
                }
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{pagina}/{qntItens}/valor/{valorMax}")]
        public IActionResult ListarPorValorMax(int pagina, int qntItens, int valorMax)
        {
            try
            {
                List<PostMinimo> posts = _marketplaceRepository.AllWhere(p => p.Valor <= valorMax && p.IsVirtual && p.IsActive, pagina, qntItens);

                if (valorMax == -1)
                {
                    return StatusCode(200, posts.OrderByDescending(p => p.Avaliacao).OrderByDescending(p => p.Vendas));
                }

                return StatusCode(200, posts.Where(p => p.Valor <= valorMax).OrderByDescending(p => p.Avaliacao).OrderByDescending(p => p.Vendas));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{pagina}/{qntItens}/comprados/{idUsuario}")]
        public IActionResult ListarJaComprados(int pagina, int qntItens, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return StatusCode(200, _marketplaceRepository.CompradosAnteriormente(pagina, qntItens, idUsuario));
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
                return StatusCode(200, _marketplaceRepository.PublicoPorUsuario(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Usuario/{idUsuario}/Meus")]
        public IActionResult ListarDeUsuario(int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return StatusCode(200, _marketplaceRepository.Meus(idUsuario));
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
                PostGenerico post = _marketplaceRepository.PublicoPorId(idPost, true);

                if (post == null)
                {
                    return NotFound(new
                    {
                        Message = "Post não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, post.IdUsuario);

                if (authResult.IsValid)
                {
                    return Ok(post);
                }

                return Ok(_marketplaceRepository.PublicoPorId(idPost, false));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("ListarPorIds")]
        public IActionResult ListarTodosPorId(List<int> IDsPosts)
        {
            try
            {
                return Ok(_marketplaceRepository.TodosPorId(IDsPosts));
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
                if (newPost == null || newPost.IdUsuario == 0)
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

                string[] extensoesPermitidas = { "jpg", "png", "jpeg", "svg, webp" };

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

                if (isSucess)
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
                Marketplace post = _marketplaceRepository.PorId(idPost, true);

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
                Marketplace post = _marketplaceRepository.PorId(idPost, true);

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
                Marketplace post = _marketplaceRepository.PorId(idPost, true);

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

                bool isSucess = _marketplaceRepository.Deletar(idPost);

                return isSucess ? StatusCode(204) : BadRequest();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
