using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Transacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacaoRepository _transacoesRepository;
        private readonly IMemoryCache _memoryCache;
        public TransacoesController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _transacoesRepository = new TransacaoRepository(ctx, memoryCache);
            _memoryCache = memoryCache;
        }

        [HttpGet("Listar/Minhas/{idUsuario}")]
        public IActionResult ListarMinhasTransacoes(int idUsuario, [FromHeader] string Authorization,
            [FromQuery] int pagina = 1,
            [FromQuery] int qntItens = 10)
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

        [HttpPost("FluxoTemporario")]
        public IActionResult FluxoTemporario(FluxoTemporarioViewModel fluxo, [FromHeader] string Authorization)
        {
            try
            {
                ExtratoTransacaoViewModel extrato = _transacoesRepository.GetFluxoFromDate(fluxo.IdUsuario, fluxo.StartDate);

                if (extrato == null)
                {
                    return NoContent();
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, fluxo.IdUsuario);

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

        [HttpPost("EfetuarTransacao")]
        public IActionResult Cadastrar(Transaco newTransacao, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, newTransacao.IdUsuarioPagante);

                if (!authResult.IsValid || newTransacao.Valor <= 0)
                {
                    return authResult.ActionResult;
                }

                UsuarioRepository _usuarioRepository = new(new digiBankContext(), _memoryCache);

                string nomePagante = _usuarioRepository.Infos(newTransacao.IdUsuarioPagante).NomeCompleto;
                string nomeRecebente = _usuarioRepository.Infos(newTransacao.IdUsuarioRecebente).NomeCompleto;

                newTransacao.Descricao = $"Transação de {nomePagante} para {nomeRecebente}";

                bool isSucess = _transacoesRepository.EfetuarTransacao(newTransacao);

                if (isSucess)
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
    }
}
