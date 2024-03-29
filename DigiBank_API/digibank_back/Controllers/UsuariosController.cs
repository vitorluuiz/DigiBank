﻿using digibank_back.Domains;
using digibank_back.DTOs;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
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

        [Authorize(Roles = "1")]
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

        [HttpGet("Publicos")]
        public IActionResult ListarPublicos()
        {
            try
            {
                return StatusCode(200, _usuariosRepository.ListarUsuariosPublicos());
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpGet("Infos/{idUsuario}")]
        public IActionResult ListarInfos(int idUsuario, [FromHeader] string Authorization) 
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                UsuarioInfos infos = _usuariosRepository.ListarInfosId(idUsuario);

                return Ok(infos);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPut("{idUsuario}")]
        public IActionResult Atualizar(int idUsuario, Usuario usuarioAtualizado, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid) 
                {
                    return authResult.ActionResult;
                }

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

        [HttpGet("{idUsuario}")]
        public IActionResult ListarPorId(int idUsuario, [FromHeader] string Authorization)
        {
            try
            {
                Usuario usuario = _usuariosRepository.ListarPorId(idUsuario);

                if (usuario == null)
                {
                    return NotFound("Usuário não existe");
                }

                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                return Ok(usuario);
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

        [Authorize(Roles = "1")]
        [HttpPatch("AddDigipoints")]
        public IActionResult AdicionarDigiPoints(PatchUsuarioSaldoViewModel patch)
        {
            try
            {
                _usuariosRepository.AdicionarDigiPoints(patch.idUsuario, patch.valor);

                decimal digipoints = (decimal)_usuariosRepository.ListarPorId(patch.idUsuario).DigiPoints;

                return Ok(digipoints);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPatch("RemoveDigipoints")]
        public IActionResult RemoverDigiPoints(PatchUsuarioSaldoViewModel patch)
        {
            try
            {
                bool isSucess = _usuariosRepository.RemoverDigiPoints(patch.idUsuario, patch.valor);

                decimal digipoints = (decimal)_usuariosRepository.ListarPorId(patch.idUsuario).DigiPoints;

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

        [Authorize(Roles = "1")]
        [HttpPatch("AddSaldo")]
        public IActionResult AdicionarSaldo(PatchUsuarioSaldoViewModel patch)
        {
            try
            {
                _usuariosRepository.AdicionarSaldo(patch.idUsuario, patch.valor);

                decimal saldo = (decimal)_usuariosRepository.ListarPorId(patch.idUsuario).Saldo;

                return Ok(saldo);
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpPatch("RemoveSaldo")]
        public IActionResult RemoverSaldo(PatchUsuarioSaldoViewModel patch)
        {
            try
            {
                bool isSucess = _usuariosRepository.RemoverSaldo((short)patch.idUsuario, patch.valor);

                decimal saldo = (decimal)_usuariosRepository.ListarPorId(patch.idUsuario).Saldo;

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
        public IActionResult Cadastrar(UsuarioMinimo newUsuario)
        {
            try
            {
                Usuario usuario = new Usuario
                {
                    NomeCompleto = newUsuario.NomeCompleto,
                    Apelido = newUsuario.Apelido,
                    Cpf = newUsuario.Cpf,
                    Telefone = newUsuario.Telefone,
                    Email = newUsuario.Email,
                    Senha = newUsuario.Senha,
                    RendaFixa = newUsuario.RendaFixa
                };

                bool isSucess = _usuariosRepository.Cadastrar(usuario);

                if (isSucess)
                {
                    return StatusCode(201, usuario);
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
        public IActionResult AlterarSenha(AlterarSenhaViewModel senha)
        {
            try
            {
                bool isSucess = _usuariosRepository.AlterarSenha(senha.idUsuario, senha.senhaAtual, senha.newSenha);

                if (isSucess)
                {
                    return Ok("Sem alterada");
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
        public IActionResult AlterarApelido(PatchUsuarioApelidoViewModel patch, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, patch.idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _usuariosRepository.AlterarApelido(patch.idUsuario, patch.newApelido);

                return Ok("Apelido atualizado");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [HttpPatch("AlterarRenda")]
        public IActionResult AlterarRenda(PatchUsuarioSaldoViewModel patch, [FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, patch.idUsuario);

                if (!authResult.IsValid)
                {
                    return authResult.ActionResult;
                }

                _usuariosRepository.AlterarRendaFixa(patch.idUsuario, patch.valor);

                return Ok(new
                {
                    RendaAtualizada = patch.valor
                });
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }

        [Authorize(Roles = "1")]
        [HttpDelete("{idUsuario}")]
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
