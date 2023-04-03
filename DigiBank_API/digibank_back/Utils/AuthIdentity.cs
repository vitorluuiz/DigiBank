using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System;

namespace digibank_back.Utils
{
    public class AuthIdentity
    {
        public static bool VerificarAcesso(string bearerToken, int idUsuario)
        {
            if(bearerToken== null)
            {
                return false;
            }

            string token = bearerToken.ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var tokenValido = handler.ValidateToken(token, new TokenValidationParameters
            {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("usuario-login-auth")),
                    ValidateIssuer = false,
                    ValidateAudience = false, 
                    ValidateLifetime = true,
             }, out SecurityToken validatedToken);

            int idToken = Convert.ToInt16(validatedToken.Id);
        
            if(idUsuario == idToken || idToken == 1)
            {
                return true;
            }

            return false;
        }
    }
}
