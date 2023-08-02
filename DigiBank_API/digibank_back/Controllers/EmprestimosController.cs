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

        //[Authorize(Roles = "1")]
        //[HttpGet]
        //public IActionResult ListarEmprestimos()
        //{
        //    try
        //    {
        //        return Ok(_emprestimoRepository.ListarTodos());
        //    }
        //    catch (Exception error)
        //    {
        //        return BadRequest(error);
        //        throw;
        //    }
        //}

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

        [HttpGet("{idEmprestimo}")]
        public IActionResult ListarPorId(int idEmprestimo, [FromHeader] string Authorization)
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

                return Ok(emprestimo);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("CalcularPagamento/{idEmprestimo}")]
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

                return Ok(_emprestimoRepository.CalcularPagamento(idEmprestimo));
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

        [HttpPost("Concluir/{idEmprestimo}")]
        public IActionResult Concluir(int idEmprestimo, [FromHeader] string Authorization)
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

                bool isSucess = _emprestimoRepository.Concluir(idEmprestimo);

                if (isSucess)
                {
                    return Ok(new
                    {
                        Message = "Empréstimo pago"
                    });
                }

                return BadRequest(new
                {
                    Message = "Não há saldo suficiente para concluir este empréstimo"
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("PagarParcela/{idEmprestimo}")]
        public IActionResult PagarParte(int idEmprestimo, decimal valor, [FromHeader] string Authorization)
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

                bool isSucess = _emprestimoRepository.ConcluirParte(idEmprestimo, valor);

                if (isSucess)
                {
                    return Ok(new
                    {
                        Message = $"{valor} reais do empréstimo foram pagos"
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
