using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class InvestimentoController : ControllerBase
    {
        private readonly IInvestimentoRepository _investimentoRepository;
        public InvestimentoController()
        {
            _investimentoRepository = new InvestimentoRepository();
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult ListarInvestimentos()
        {
            try
            {
                return Ok(_investimentoRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Id/{idInvestimento}")]
        public IActionResult ListarPorId(int idInvestimento, [FromHeader] string Authorization) 
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);

                if(investimento == null) 
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if(isAcessful)
                {
                    return Ok(investimento);
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

        [HttpGet("IdUsuario/{idUsuario}")]
        public IActionResult ListarDeUsuario(int idUsuario, [FromHeader] string Authorization) 
        {
            try
            {
                List<Investimento> investimentos = _investimentoRepository.ListarDeUsuario(idUsuario);

                if (investimentos == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (isAcessful)
                {
                    return Ok(investimentos);
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

        [HttpGet("PreverGanhos/{idInvestimento}/{diasInvestidos}")]
        public IActionResult Prever(int idInvestimento, int diasInvestidos, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);
                PreviewRentabilidade rentabilidade = _investimentoRepository.CalcularPrevisao(idInvestimento, diasInvestidos);

                if (rentabilidade == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if (isAcessful)
                {
                    return Ok(investimento);
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

        [HttpGet("CalcularGanhos/{idInvestimento}")]
        public IActionResult CalcularGanhos(int idInvestimento, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);
                PreviewRentabilidade rentabilidade = _investimentoRepository.CalcularGanhos(idInvestimento);

                if (rentabilidade == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if (isAcessful)
                {
                    return Ok(investimento);
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

        [HttpPost("Comprar")]
        public IActionResult Comprar(Investimento newInvestimento, int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

                newInvestimento.DataAquisicao = DateTime.Now;

                bool isSucess = _investimentoRepository.Comprar(newInvestimento, idUsuario);

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

        [HttpPost("Vender")]
        public IActionResult Vender(int idInvestimento, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if(investimento == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
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
        public IActionResult VenderCotas(int idInvestimento, decimal qntCotas, [FromHeader] string Authorization)
        {
            try
            {
                Investimento investimento = _investimentoRepository.ListarPorId(idInvestimento);

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, investimento.IdUsuario);

                if (investimento == null)
                {
                    return NotFound(new
                    {
                        Message = "Investimento não existe"
                    });
                }

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

                _investimentoRepository.VenderCotas(idInvestimento, qntCotas);

                return Ok(new
                {
                    Message = $"Venda de {qntCotas} realizada"
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
