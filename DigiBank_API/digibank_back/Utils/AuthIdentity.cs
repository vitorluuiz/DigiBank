using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;
using digibank_back.Domains;
using System.Linq;
using System.Security.Claims;

namespace digibank_back.Utils
{
    public class AuthIdentityResult
    {
        public bool IsValid { get; set; }
        public IActionResult ActionResult { get; set; }
        public string NewToken { get; set; }
    }

    public class AuthIdentity
    {
        public static AuthIdentityResult VerificarAcesso(string bearerToken, int idUsuario)
        {
            if (bearerToken == null)
            {
                return new AuthIdentityResult
                {
                    IsValid = false,
                    ActionResult = CreateHttpResponse(StatusCodes.Status401Unauthorized, "Token de acesso não fornecido"),
                };
            }

            string token = bearerToken.ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            try
            {
                var tokenValido = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("usuario-login-auth")),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                }, out SecurityToken validatedToken);

                int idToken = Convert.ToInt32(validatedToken.Id);

                if (idUsuario == idToken || idToken == 1)
                {
                    return new AuthIdentityResult
                    {
                        IsValid = true,
                        ActionResult = new OkResult()
                    };
                }
                else if(idUsuario == -1)
                {
                    throw new SecurityTokenExpiredException();
                }
                else if(idUsuario == -2)
                {
                    //Acionar uma exception personalizada
                    throw new SecurityTokenExpiredException();
                }
                else
                {
                    return new AuthIdentityResult
                    {
                        IsValid = false,
                        ActionResult = CreateHttpResponse(StatusCodes.Status403Forbidden, "Acesso negado")
                    };
                }
            }
            catch (SecurityTokenExpiredException)
            {
                var tokenValido = handler.ReadJwtToken(token);
                string novoToken = RenovarToken(tokenValido);

                return new AuthIdentityResult
                {
                    IsValid = false,
                    ActionResult = CreateHttpResponse(StatusCodes.Status401Unauthorized, "Token de acesso expirado"),
                    NewToken = novoToken
                };
            }
            catch (SecurityTokenException)
            {
                return new AuthIdentityResult
                {
                    IsValid = false,
                    ActionResult = CreateHttpResponse(StatusCodes.Status401Unauthorized, "Token de acesso inválido")
                };
            }
            //Verificar se ela foi acionada
            catch (Exception ex)
            {
                return new AuthIdentityResult
                {
                    IsValid = false,
                    ActionResult = CreateHttpResponse(StatusCodes.Status500InternalServerError, "Ocorreu um erro durante a validação do token de acesso: " + ex.Message)
                };
            }
        }
        private static string RenovarToken(JwtSecurityToken jwtToken)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(30);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("usuario-login-auth"));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string audience = null;
            if (jwtToken.Payload.ContainsKey("aud"))
            {
                audience = jwtToken.Payload["aud"].ToString();
            }

            string sub = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            string jti = jwtToken.Claims.FirstOrDefault(c => c.Type == "jti")?.Value;
            string role = jwtToken.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

            var minhasClaims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, sub),
                    new Claim(JwtRegisteredClaimNames.Jti, jti),
                    new Claim("role", role)
                };

            var newToken = new JwtSecurityToken(
                issuer: jwtToken.Issuer,
                audience: audience,
                expires: expiration,
                claims: minhasClaims,
                signingCredentials: signingCredentials
            );

            return tokenHandler.WriteToken(newToken);
        }

        private static IActionResult CreateHttpResponse(int statusCode, string message)
        {
            return new ObjectResult(message)
            {
                StatusCode = statusCode,
                ContentTypes = { "text/plain" }
            };
        }
    }
}
