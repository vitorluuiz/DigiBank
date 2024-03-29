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
                return Ok(_avaliacaoRepository.AvaliacoesPost(idPost, pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult CadastrarAvaliacao(Avaliaco newAvaliacao, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, (int)newAvaliacao.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _avaliacaoRepository.Cadastrar(newAvaliacao);

                return StatusCode(201);
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
