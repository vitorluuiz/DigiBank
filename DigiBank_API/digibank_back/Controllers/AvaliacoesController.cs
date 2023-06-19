﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Repositories;
using digibank_back.Utils;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using digibank_back.ViewModel.Avaliacao;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IAvaliacaoRepository _avaliacaoRepository;

        public AvaliacoesController()
        {
            _avaliacaoRepository = new AvaliacaoRepository();
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

        [HttpGet("AvaliacoesPost/{idPost}/{pagina}/{qntItens}")]
        public IActionResult AvaliacoesPost(int idPost, int pagina, int qntItens)
        {
            try
            {
                return Ok(new
                {
                    AvaliacoesList = _avaliacaoRepository.AvaliacoesPost(idPost, pagina, qntItens),
                    RatingHistograma = _avaliacaoRepository.CountAvaliacoesRating(idPost)
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
                    IdPost = (byte?)newAvaliacao.IdPost,
                    IdUsuario = (short?)newAvaliacao.IdUsuario,
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
                    Message = "Usuário não possuí o produto"
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

                if(avaliacao == null)
                {
                    return NotFound(new
                    {
                        Message = "Avaliação não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idAvaliacao);

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

                return Ok( new
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
