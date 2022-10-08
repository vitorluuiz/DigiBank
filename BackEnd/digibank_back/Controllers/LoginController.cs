using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController()
        {
            _usuarioRepository = new UsuarioRepository();
        }

        [HttpPost("Logar")]
        public IActionResult Login(string cpf, string senha)
        {
            try
            {
                Usuario usuarioLogado = _usuarioRepository.Login(cpf, senha);

                if(usuarioLogado != null)
                {
                    var minhasClaims = new[]
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, usuarioLogado.Cpf),
                    new Claim(JwtRegisteredClaimNames.Jti,usuarioLogado.IdUsuario.ToString())
                    };

                    var Key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("usuario-login-auth"));

                    var Creds = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "digiBank.WebApi",
                        audience: "digiBank.WebApi",
                        claims: minhasClaims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: Creds
                        );

                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                return BadRequest("Usuário não encontrado");
            }
            catch (Exception error)
            {
                return BadRequest(error);
                throw;
            }
        }
    }
}
