using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Investimento;
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
    public class InvestimentoController : ControllerBase
    {
        private readonly digiBankContext _ctx;
        private readonly IInvestimentoRepository _investimentoRepository;
        private readonly HistoryInvestRepository _historyInvestRepository;
        private readonly IMemoryCache _memoryCache;
        public InvestimentoController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _ctx = ctx;
            _memoryCache = memoryCache;
            _investimentoRepository = new InvestimentoRepository(ctx, memoryCache);
            _historyInvestRepository = new HistoryInvestRepository(ctx, memoryCache);
        }

        [Authorize(Roles = "1")]
        [HttpPost("CriarCarteira")]
        public IActionResult CriarCarteira(CarteiraViewModel carteira)
        {
            try
            {
                var mockInvestimento = new MockData.Investimento(_ctx, _historyInvestRepository, new InvestimentoRepository(_ctx, _memoryCache));
                mockInvestimento.CreateCarteira(carteira.IdUsuario, carteira.Valor, carteira.Inicio, carteira.Fim);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Usuario/{idUsuario}/{pagina}/{qntItens}")]
        public IActionResult ListarDeUsuario(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(new
                {
                    investimentosList = _investimentoRepository.AllWhere(o => o.IdUsuario == idUsuario, pagina, qntItens),
                    Count = _investimentoRepository.CountWhere(o => o.IdUsuario == idUsuario)
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Comprar")]
        public IActionResult Comprar(InvestimentoViewModel newInvestimento, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, newInvestimento.IdUsuarioPagante);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucess = _investimentoRepository.Comprar(new Investimento
                {
                    IdUsuario = newInvestimento.IdUsuarioPagante,
                    IdInvestimentoOption = newInvestimento.IdInvestimentoOption,
                    QntCotas = newInvestimento.Cotas
                });

                if (isSucess)
                {
                    return StatusCode(201, new
                    {
                        Message = "Compra realizada"
                    });
                }

                return BadRequest(new
                {
                    Message = "Não foi possível realizar a compra"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("VenderCotas")]
        public IActionResult VenderCotas(VendaCotasViewModel venda, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(venda.IdIvestimento);

                if (investimento == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _investimentoRepository.VenderCotas(venda.IdIvestimento, venda.QntCotas);

                return Ok(new
                {
                    Message = $"Venda de {venda.QntCotas} realizada"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Investido/{idUsuario}")]
        public IActionResult ListarExtratoInvestimentos(int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_investimentoRepository.ExtratoTotalInvestido(idUsuario));

            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
