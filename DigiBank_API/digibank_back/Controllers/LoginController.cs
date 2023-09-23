using digibank_back.Contexts;
using digibank_back.Domains;
using digibank_back.Interfaces;
using digibank_back.Repositories;
using digibank_back.Utils;
using digibank_back.ViewModel.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace digibank_back.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController(digiBankContext ctx, IMemoryCache memoryCache)
        {
            _usuarioRepository = new UsuarioRepository(ctx, memoryCache);
        }

        [HttpPost("Logar")]
        public IActionResult Login(LoginViewModel login)
        {
            try
            {
                Usuario usuarioLogado = _usuarioRepository.Login(login.cpf, login.senha);

                if (usuarioLogado != null)
                {
                    var minhasClaims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, usuarioLogado.Cpf),
                        new Claim(JwtRegisteredClaimNames.Jti,usuarioLogado.IdUsuario.ToString()),
                        new Claim("role", usuarioLogado.IdUsuario.ToString())
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

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken([FromHeader] string Authorization)
        {
            try
            {
                AuthIdentityResult authResult = AuthIdentity.VerificarAcesso(Authorization, -1); //Id é definido como -1, que representa um Id não específicado

                if (authResult.NewToken != null)
                {
                    return Ok(new
                    {
                        Token = authResult.NewToken,
                    });
                }

                return BadRequest(new
                {
                    Message = "Não foi possível renovar o token"
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
