using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Utils;
using digibank_back.ViewModel.Emprestimo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoRepository _emprestimoRepository;

        public EmprestimosController(IEmprestimoRepository emprestimoRepository)
        {
            _emprestimoRepository = emprestimoRepository;
        }

        [HttpGet("IdUsuario/{idUsuario}")]
        public IActionResult ListarDeUsuario(int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_emprestimoRepository.ListarDeUsuario(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Simular/{idEmprestimo}")]
        public IActionResult Calcular(int idEmprestimo, [FromHeader] string Authorization)
        {
            try
            {
                Emprestimo emprestimo = _emprestimoRepository.ListarPorId(idEmprestimo);

                if (emprestimo == null)
                {
                    return NotFound(new
                    {
                        Message = "Empréstimo não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, emprestimo.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(_emprestimoRepository.Simular(idEmprestimo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Atribuir(EmprestimoViewModel newEmprestimo, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, newEmprestimo.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                Emprestimo emprestimo = new Emprestimo
                {
                    IdUsuario = (short)newEmprestimo.IdUsuario,
                    IdEmprestimoOptions = (byte)newEmprestimo.IdEmprestimoOptions
                };

                bool isSucess = _emprestimoRepository.Atribuir(emprestimo);

                if (isSucess)
                {
                    return StatusCode(201);
                }

                return BadRequest(new
                {
                    Message = "Renda fixa não permite obter este empréstimo"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("PagarParcela")]
        public IActionResult PagarParte(PagarEmprestimoViewModel pagamento, [FromHeader] string Authorization)
        {
            try
            {
                Emprestimo emprestimo = _emprestimoRepository.ListarPorId(pagamento.IdEmprestimo);

                if (emprestimo == null)
                {
                    return NotFound(new
                    {
                        Message = "Empréstimo não existe"
                    });
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, emprestimo.IdUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                bool isSucess = _emprestimoRepository.ConcluirParte(pagamento.IdEmprestimo, pagamento.Valor);

                if (isSucess)
                {
                    Emprestimo updatedEmprestimo = _emprestimoRepository.ListarPorId(pagamento.IdEmprestimo);

                    return Ok(new
                    {
                        Message = $"{updatedEmprestimo.ValorPago} reais do empréstimo foram pagos",
                        Emprestimo = updatedEmprestimo
                    });
                }

                return BadRequest(new
                {
                    Message = "Não há saldo suficiente para pagar o valor desejado do empréstimo"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPatch("EstenderPrazo")]
        public IActionResult EstenderPrazo(EstenderEmprestimoViewModel prazo)
        {
            try
            {
                _emprestimoRepository.EstenderPrazo(prazo.IdEmprestmo, prazo.NovoPrazo);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
