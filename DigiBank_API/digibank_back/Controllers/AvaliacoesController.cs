using System;
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

        [HttpGet("Id/{idAvaliacao}")]
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
        public IActionResult CadastrarAvaliacao(Avaliaco newAvaliacao)
        {
            try
            {
                    _avaliacaoRepository.Cadastrar(newAvaliacao);

                    return StatusCode(201);
            }
            catch (Exception error) 
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("Id/{idAvaliacao}")]
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

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, (int)avaliacao.IdUsuario);

                if(isAcessful)
                {
                    _avaliacaoRepository.Deletar(idAvaliacao);

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

        [HttpPut("Id/{idAvaliacao}")]
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

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, (int)avaliacao.IdUsuario);

                if (isAcessful)
                {
                    _avaliacaoRepository.AtualizarAvaliacao(idAvaliacao, avaliacaoAtualizada);

                    return Ok( new
                    {
                        Message = "Avaliação atualizada"
                    });
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
