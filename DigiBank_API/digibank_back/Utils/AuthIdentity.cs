using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;
using Microsoft.AspNetCore.Mvc;

namespace digibank_back.Utils
{
    public class AuthIdentityResult
    {
        public bool IsValid { get; set; }
        public IActionResult ActionResult { get; set; }
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
                    ActionResult = CreateHttpResponse(StatusCodes.Status401Unauthorized, "Token de acesso não fornecido")
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
                return new AuthIdentityResult
                {
                    IsValid = false,
                    ActionResult = CreateHttpResponse(StatusCodes.Status401Unauthorized, "Token de acesso expirado")
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
            catch (Exception ex)
            {
                return new AuthIdentityResult
                {
                    IsValid = false,
                    ActionResult = CreateHttpResponse(StatusCodes.Status500InternalServerError, "Ocorreu um erro durante a validação do token de acesso: " + ex.Message)
                };
            }
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
