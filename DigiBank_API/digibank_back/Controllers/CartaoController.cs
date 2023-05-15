using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Cartao;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class CartaoController : ControllerBase
    {
        private readonly ICartaoRepository _cartaoRepository;
        public CartaoController()
        {
            _cartaoRepository = new CartaoRepository();
        }

        [HttpGet("Usuario/{idUsuario}")]
        public IActionResult GetCartao(int idUsuario)
        {
            try
            {
                return Ok(_cartaoRepository.GetCartoes(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("GerarCartao")]
        public IActionResult GerarCartao(Cartao newCartao)
        {
            try
            {
                return StatusCode(201, _cartaoRepository.Gerar(newCartao));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("{idCartao}")]
        public IActionResult DeletarCartao(int idCartao)
        {
            try
            {
                _cartaoRepository.Excluir(idCartao);
                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("EfetuarPagamento")]
        public IActionResult EfetuarPagamento(CartaoViewModel cartaoModel)
        {
            try
            {
                bool isSucess = _cartaoRepository.EfetuarPagamento(cartaoModel);

                if (!isSucess)
                {
                    return BadRequest(new
                    {
                        Message = "Pagamento não efetuado"
                    });
                }

                return Ok(new
                {
                    Message = "Pagamento realizado"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("Bloquear/{idCartao}")]
        public IActionResult Bloquear(int idCartao)
        {
            try
            {
                bool isSucess = _cartaoRepository.Bloquear(idCartao);

                if (isSucess)
                {
                    return Ok(new
                    {
                        Message = "Cartão bloqueado"
                    });
                }

                return BadRequest(new
                {
                    Message = "Não foi possível bloquear"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("Desbloquear/{idCartao}")]
        public IActionResult Desbloquear(int idCartao)
        {
            try
            {
                bool isSucess = _cartaoRepository.Desbloquear(idCartao);

                if (isSucess)
                {
                    return Ok(new
                    {
                        Message = "Cartão desbloqueado"
                    });
                }

                return BadRequest(new
                {
                    Message = "Não foi possível desbloquear"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("ALterarSenha/{idCartao}")]
        public IActionResult AlterarSenha(int idCartao, SenhaCartao newToken, [FromHeader] string Authorization)
        {
            try
            {
                Cartao cartao = _cartaoRepository.ListarPorID(idCartao);

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, cartao.IdUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

                bool isSucess = _cartaoRepository.AlterarSenha(idCartao, newToken.newToken);

                if (isSucess)
                {
                    return Ok(new
                    {
                        Message = "Senha alterada"
                    });
                }

                return BadRequest(new
                {
                    Message = "Senha atual não confere"
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
