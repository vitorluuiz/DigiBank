using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
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
        public EmprestimosController()
        {
            _emprestimoRepository= new EmprestimoRepository();
        }

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
        public IActionResult ListarDeUsuario(int idUsuario) 
        {
            try
            {
                return Ok(_emprestimoRepository.ListarDeUsuario(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Id/{idEmprestimo}")]
        public IActionResult ListarPorId(int idEmprestimo)
        {
            try
            {
                return Ok(_emprestimoRepository.ListarPorId(idEmprestimo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("CalcularPagemento/{idEmprestimo}")]
        public IActionResult Calcular(int idEmprestimo)
        {
            try
            {
                return Ok(_emprestimoRepository.CalcularPagamento(idEmprestimo));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Atribuir(Emprestimo newEmprestimo)
        {
            try
            {
                bool isSucess = _emprestimoRepository.Atribuir(newEmprestimo);

                if(isSucess)
                {
                    return Ok();
                }

                return BadRequest("Sua renda fixa não permite o pedido deste emprestimo");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("Concluir/{idEmprestimo}")]
        public IActionResult Concluir(int idEmprestimo)
        {
            try
            {
                bool isSucess = _emprestimoRepository.Concluir(idEmprestimo);

                if (isSucess)
                {
                    return Ok();
                }

                return BadRequest("Saldo insuficiente para quitar o emprestimo");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost("PagarParcela/{idEmprestimo}")]
        public IActionResult PagarParte(int idEmprestimo, decimal valor)
        {
            try
            {
                bool isSucess = _emprestimoRepository.ConcluirParte(idEmprestimo, valor);

                if (isSucess)
                {
                    return Ok();
                }

                return BadRequest("Saldo insuficiente para quitar o emprestimo");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

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
