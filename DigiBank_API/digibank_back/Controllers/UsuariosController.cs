using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http.Json;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _usuariosRepository;

        public UsuariosController()
        {
            _usuariosRepository = new UsuarioRepository();
        }

        [HttpGet]
        public IActionResult ListarTodos()
        {
            try
            {
                return StatusCode(200, _usuariosRepository.ListarTodos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPut("Id/{idUsuario}")]
        public IActionResult Atualizar(int idUsuario, Usuario usuarioAtualizado)
        {
            try
            {
                bool isUpdated = _usuariosRepository.Atualizar(idUsuario, usuarioAtualizado);

                if (isUpdated)
                {
                    return Ok();
                }

                return BadRequest();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Id/{idUsuario}")]
        public IActionResult ListarId(int idUsuario)
        {
            try
            {
                return StatusCode(200, _usuariosRepository.ListarPorId(idUsuario));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Cpf/{cpf}")]
        public IActionResult ListarCpf(string cpf)
        {
            try
            {
                return StatusCode(200, _usuariosRepository.ListarPorCpf(cpf));
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AddDigiPoints")]
        public IActionResult AdicionarDigiPoints(int idUsuario, decimal valor)
        {
            try
            {
                _usuariosRepository.AdicionarDigiPoints(idUsuario, valor);

                decimal digipoints = (decimal)_usuariosRepository.ListarPorId(idUsuario).DigiPoints;

                return Ok(digipoints);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("RemoveDigiPoints")]
        public IActionResult RemoverDigiPoints(int idUsuario, decimal valor)
        {
            try
            {
                bool isSucess = _usuariosRepository.RemoverDigiPoints(idUsuario, valor);

                decimal digipoints = (decimal)_usuariosRepository.ListarPorId(idUsuario).DigiPoints;

                if (isSucess)
                {
                    return Ok(digipoints);
                }

                return BadRequest("Saldo insuficiente");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AddSaldo")]
        public IActionResult AdicionarSaldo(int idUsuario, decimal valor)
        {
            try
            {
                _usuariosRepository.AdicionarSaldo(idUsuario, valor);

                decimal saldo = (decimal)_usuariosRepository.ListarPorId(idUsuario).Saldo;

                return Ok(saldo);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("RemoveSaldo")]
        public IActionResult RemoverSaldo(short idUsuario, decimal valor)
        {
            try
            {
                bool isSucess = _usuariosRepository.RemoverSaldo(idUsuario, valor);

                decimal saldo = (decimal)_usuariosRepository.ListarPorId(idUsuario).Saldo;

                if (isSucess)
                {
                    return Ok(saldo);
                }

                return BadRequest("Saldo insuficiente");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Cadastrar(Usuario newUsuario)
        {
            try
            {
                bool isSucess = _usuariosRepository.Cadastrar(newUsuario);

                if (isSucess)
                {
                    return StatusCode(201, newUsuario);
                }

                return BadRequest("Usuário já existe");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AlterarSenha")]
        public IActionResult AlterarSenha(int idUsuario,string senhaAtual, string newSenha)
        {
            try
            {
                bool isSucess = _usuariosRepository.AlterarSenha(idUsuario, senhaAtual, newSenha);

                if (isSucess)
                {
                    return Ok();
                }
                return BadRequest("Senha atual não é válida");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AlterarApelido")]
        public IActionResult AlterarApelido(int idUsuario, string newApelido)
        {
            try
            {
                _usuariosRepository.AlterarApelido(idUsuario, newApelido);

                return Ok();
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AlterarRenda")]
        public IActionResult AlterarRenda(int idUsuario, decimal renda)
        {
            try
            {
                _usuariosRepository.AlterarRendaFixa(idUsuario, renda);

                return Ok(renda);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpDelete("Id/{idUsuario}")]
        public IActionResult Delete(int idUsuario) {
            try
            {
                bool isSucess = _usuariosRepository.Deletar(idUsuario);

                if(isSucess)
                {
                    return StatusCode(204);
                }
                else
                {
                    return BadRequest("Não foi possível deletar o usuário, talvez existam dependências com outras entidades do banco de dados");
                }
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
