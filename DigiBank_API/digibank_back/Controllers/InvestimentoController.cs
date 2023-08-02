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
        private readonly IInvestimentoRepository _investimentoRepository;
        public InvestimentoController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _investimentoRepository = new InvestimentoRepository(ctx, memoryCache);
        }

        //[Authorize(Roles = "1")]
        //[HttpGet]
        //public IActionResult ListarInvestimentos()
        //{
        //    try
        //    {
        //        return Ok(_investimentoRepository.ListarTodos());
        //    }
        //    catch (Exception error)
        //    {
        //        return BadRequest(error);
        //        throw;
        //    }
        //}

        [HttpGet("{idInvestimento}")]
        public IActionResult ListarPorId(int idInvestimento, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);

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

                return Ok(investimento);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("IdUsuario/{idUsuario}")]
        public IActionResult ListarDeUsuario(int idUsuario, int pagina, int qntItens, [FromHeader] string Authorization)
        {
            try
            {
                List<Investimento> investimentos = _investimentoRepository.ListarDeUsuario(idUsuario, pagina, qntItens);

                if (investimentos == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(investimentos);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Comprar/{idUsuario}")]
        public IActionResult Comprar(Investimento newInvestimento, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                newInvestimento.DataAquisicao = DateTime.Now;

                bool isSucess = _investimentoRepository.Comprar(newInvestimento);

                if (isSucess)
                {
                    return Ok(new
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

        [HttpPost("Vender/{idInvestimento}")]
        public IActionResult Vender(int idInvestimento, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                if (investimento == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                _investimentoRepository.Vender(idInvestimento);

                return Ok(new
                {
                    Message = "Compra realizada"
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
