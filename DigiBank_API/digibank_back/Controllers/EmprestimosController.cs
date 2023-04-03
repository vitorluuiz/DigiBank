using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Net;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEmprestimoRepository _emprestimoRepository;
        public EmprestimosController()
        {
            _emprestimoRepository= new EmprestimoRepository();
        }

        [Authorize(Roles = "1")]
        [HttpGet]
        public IActionResult ListarEmprestimos()
        {
            try
            {
                return Ok(_emprestimoRepository.ListarTodos());
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
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (isAcessful)
                {
                    return Ok(_emprestimoRepository.ListarDeUsuario(idUsuario));
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

        [HttpGet("Id/{idEmprestimo}")]
        public IActionResult ListarPorId(int idEmprestimo, [FromHeader] string Authorization)
        {
            try
            {
                Emprestimo emprestimo = _emprestimoRepository.ListarPorId(idEmprestimo);

                if(emprestimo == null)
                {
                    return NotFound(new
                    {
                        Message = "Empréstimo não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, emprestimo.IdUsuario);

                if (isAcessful)
                {
                    return Ok(emprestimo);
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

        [HttpGet("CalcularPagemento/{idEmprestimo}")]
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

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, emprestimo.IdUsuario);

                if (isAcessful)
                {
                    return Ok(_emprestimoRepository.CalcularPagamento(idEmprestimo));
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

        [HttpPost]
        public IActionResult Atribuir(Emprestimo newEmprestimo, [FromHeader] string Authorization)
        {
            try
            {
                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, newEmprestimo.IdUsuario);

                if(!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
                }

                bool isSucess = _emprestimoRepository.Atribuir(newEmprestimo);

                if(isSucess)
                {
                    return Ok();
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

                if(emprestimo == null)
                {
                    return NotFound(new
                    {
                        Message = "Empréstimo não existe"
                    });
                }

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, emprestimo.IdUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
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

                bool isAcessful = AuthIdentity.VerificarAcesso(Authorization, emprestimo.IdUsuario);

                if (!isAcessful)
                {
                    return StatusCode(403, new
                    {
                        Message = "Sem acesso"
                    });
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
        [HttpPatch("ExtenderPrazo")]
        public IActionResult ExtenderPrazo(int idEmprestimo, DateTime newPrazo) 
        {
            try
            {
                _emprestimoRepository.EstenderPrazo(idEmprestimo, newPrazo);

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
