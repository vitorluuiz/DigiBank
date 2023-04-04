using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using digibank_back.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Net;
using digibank_back.DTOs;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoRepository _transacoesRepository;
        public TransacoesController()
        {
            _transacoesRepository = new TransacaoRepository();
        }

        [Authorize(Roles = "1")]
        [HttpGet("{pagina}/{qntItens}")]
        public IActionResult ListarTodas(int pagina, int qntItens)
        {
            try
            {
                return Ok(_transacoesRepository.ListarTodas(pagina, qntItens));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("{idTransacao}")]
        public IActionResult ListarPorId(int idTransacao, [FromHeader] string Authorization)
        {
            try
            {
                Transaco transacao = _transacoesRepository.ListarPorid(idTransacao);

                if(transacao == null) 
                {
                    return NoContent();
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, transacao.IdUsuarioPagante);

                if (!isAcessful)
                {
                    isAcessful = AuthIdentity.VerificarAcesso(Authorization, transacao.IdUsuarioRecebente);
                }

                if(isAcessful)
                {
                    return Ok(transacao);
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

        [HttpGet("Recebidas/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult ListarRecebidas(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                List<TransacaoGenerica> transacoes = _transacoesRepository.ListarRecebidas(idUsuario, pagina, qntItens);

                if(transacoes == null)
                {
                    return NoContent();
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (isAcessful)
                {
                    return Ok(transacoes);
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

        [HttpGet("Enviadas/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult ListarEnviadas(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                List<TransacaoGenerica> transacoes = _transacoesRepository.ListarEnviadas(idUsuario, pagina, qntItens);

                if (transacoes == null)
                {
                    return NoContent();
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (isAcessful)
                {
                    return Ok(transacoes);
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

        [HttpGet("FluxoEntreUsuarios/{idPagante}/{idRecebente}")]
        public IActionResult FluxoTotal(int idPagante, int idRecebente, [FromHeader] string Authorization)
        {
            try
            {
                ExtratoTransacaoViewModel extrato = _transacoesRepository.FluxoTotal(idPagante, idRecebente);

                if (extrato == null)
                {
                    return NoContent();
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idPagante);

                if (!isAcessful)
                {
                    isAcessful = AuthIdentity.VerificarAcesso(Authorization, idRecebente);
                }

                if (isAcessful)
                {
                    return Ok(extrato);
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

        [HttpGet("EntreUsuarios/{idUsuario1}/{idUsuario2}/{pagina}/{qntItens}")]
        public IActionResult ListarEntreUsuarios(int idUsuario1, int idUsuario2, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                List<TransacaoGenerica> transacoes = _transacoesRepository.ListarEntreUsuarios(idUsuario1, idUsuario2, pagina, qntItens);

                if (transacoes == null)
                {
                    return NoContent();
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario1);

                if (!isAcessful)
                {
                    isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario2);
                }

                if (isAcessful)
                {
                    return Ok(transacoes);
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

        [HttpPost("EfetuarTransacao")]
        public IActionResult Cadastrar(Transaco newTransacao, [FromHeader] string Authorization)
        {
            try
            {
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, newTransacao.IdUsuarioPagante);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

                bool isSucess = _transacoesRepository.EfetuarTransacao(newTransacao);

                if(isSucess)
                {
                    return StatusCode(201);
                }

                return BadRequest("Saldo insuficiente");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("{idTransacao}")]
        public IActionResult Deletar(int idTransacao, [FromHeader] string Authorization)
        {
            try
            {
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, 0);

                if (isAcessful)
                {
                    _transacoesRepository.Deletar(idTransacao);
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
