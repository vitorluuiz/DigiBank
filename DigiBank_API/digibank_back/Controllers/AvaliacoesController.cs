using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Avaliacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public AvaliacoesController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _avaliacaoRepository = new AvaliacaoRepository(ctx, memoryCache);
        }

        [Authorize(Roles = "1")]
        [HttpGet("{pagina}/{qntItens}")]
        public IActionResult ListarTodos(int pagina, int qntItens)
        {
            try
            {
                return Ok(_avaliacaoRepository.ListarTodas(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idAvaliacao}")]
        public IActionResult ListarPorId(int idAvaliacao)
        {
            try
            {
                return Ok(_avaliacaoRepository.ListarPorId(idAvaliacao));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("AvaliacoesPost/{idPost}/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult AvaliacoesPost(int idPost, int idUsuario, int pagina, int qntItens)
        {
            try
            {
                return Ok(new
                {
                    AvaliacoesList = _avaliacaoRepository.AvaliacoesPost(idPost, idUsuario, pagina, qntItens),
                    RatingHistograma = _avaliacaoRepository.CountAvaliacoesRating(idPost),
                    CanPostComment = _avaliacaoRepository.HasCommentsRights(idUsuario, idPost)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult CadastrarAvaliacao(AvaliacaoViewModel newAvaliacao, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, newAvaliacao.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                Avaliaco avaliacao = new Avaliaco
                {
                    IdAvaliacao = (short)newAvaliacao.IdAvaliacao,
                    IdPost = (byte)newAvaliacao.IdPost,
                    IdUsuario = newAvaliacao.IdUsuario,
                    Comentario = newAvaliacao.Comentario,
                    Nota = newAvaliacao.Nota
                };

                bool isSucess = _avaliacaoRepository.Cadastrar(avaliacao);

                if (isSucess)
                {
                    return StatusCode(201);
                }

                return BadRequest(new
                {
                    Message = "Usuário não possuí o produto, ou já tem uma resenha cadastrada"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("{idAvaliacao}")]
        public IActionResult DeletarAvaliacao(int idAvaliacao, [FromHeader] string Authorization)
        {
            try
            {
                Avaliaco avaliacao = _avaliacaoRepository.ListarPorId(idAvaliacao);

                if (avaliacao == null)
                {
                    return NotFound(new
                    {
                        Message = "Avaliação não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)avaliacao.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _avaliacaoRepository.Deletar(idAvaliacao);
                return StatusCode(204);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("Like/{idAvaliacao}/{idUsuario}")]
        public IActionResult Curtir(int idAvaliacao, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucess = _avaliacaoRepository.AddLike(idAvaliacao, idUsuario);

                if (isSucess)
                {
                    return Ok();
                }

                return BadRequest(new
                {
                    message = "Não foi possível cadastrar"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("UnLike/{idAvaliacao}/{idUsuario}")]
        public IActionResult RemoverCurtida(int idAvaliacao, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucess = _avaliacaoRepository.RemoveLike(idAvaliacao, idUsuario);

                if (isSucess)
                {
                    return Ok();
                }

                return BadRequest(new
                {
                    message = "Comentário não existe"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPut("{idAvaliacao}")]
        public IActionResult Atualizar(int idAvaliacao, Avaliaco avaliacaoAtualizada, [FromHeader] string Authorization)
        {
            try
            {
                Avaliaco avaliacao = _avaliacaoRepository.ListarPorId(idAvaliacao);

                if (avaliacao == null)
                {
                    return NotFound(new
                    {
                        Message = "Avaliação não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)avaliacao.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _avaliacaoRepository.AtualizarAvaliacao(idAvaliacao, avaliacaoAtualizada);

                return Ok(new
                {
                    Message = "Avaliação atualizada"
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
