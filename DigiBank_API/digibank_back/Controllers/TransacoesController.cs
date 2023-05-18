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

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, transacao.IdUsuarioPagante);


                if (!authResult.IsValid)
                {
                    authResult = AuthIdentity.VerificarAcesso(Authorization, transacao.IdUsuarioRecebente);
                }

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(transacao);
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

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(transacoes);
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

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(transacoes);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
        [HttpGet("Listar/Minhas/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult ListarMinhasTransacoes(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                int qntTransacoes = _transacoesRepository.QntTransacoesUsuario(idUsuario);
                TransacaoCount listaTransacoes = _transacoesRepository.ListarMinhasTransacoes(idUsuario, pagina, qntItens);

                if (listaTransacoes == null)
                {
                    return NoContent();
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(listaTransacoes);
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

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idPagante);

                if (!authResult.IsValid)
                {
                   authResult = AuthIdentity.VerificarAcesso(Authorization, idRecebente);
                }

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(extrato);
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

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario1);

                if (!authResult.IsValid)
                {
                    authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario2);
                }

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }


                return Ok(transacoes);
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
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, newTransacao.IdUsuarioPagante);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                UsuarioRepository _usuarioRepository = new UsuarioRepository();

                string nomePagante = _usuarioRepository.ListarInfosId(newTransacao.IdUsuarioPagante).NomeCompleto;
                string nomeRecebente = _usuarioRepository.ListarInfosId(newTransacao.IdUsuarioRecebente).NomeCompleto;

                newTransacao.Descricao = $"Transação de {nomePagante} para {nomeRecebente}";

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
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, 0);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _transacoesRepository.Deletar(idTransacao);
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
