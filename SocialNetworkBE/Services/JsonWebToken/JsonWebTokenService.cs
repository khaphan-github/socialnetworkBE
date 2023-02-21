using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace SocialNetworkBE.Services.JsonWebToken {
    public class JsonWebTokenService {
        // https://gist.github.com/rafiulgits/b967c3f1716436b3c59d038d04a51009

        // TODO: Handle Secure Secretkey
        private const string SECRET_KEY = "Secretkey";
        public string CreateTokenFromUserData(ClaimsIdentity claimsIdentity, int exprisesTime) {

            byte[] secretKey = Encoding.UTF8.GetBytes(SECRET_KEY);

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(secretKey);

            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512Signature
            );

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor {
                SigningCredentials = signingCredentials,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(exprisesTime)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(token);
        }

        private static ClaimsPrincipal GetPrincipalFromToken(string token) {
            try {
                JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                JwtSecurityToken jwtToken = jwtSecurityTokenHandler.ReadJwtToken(token);

                if (jwtToken == null) {
                    return null;
                }

                byte[] secretKey = Encoding.UTF8.GetBytes(SECRET_KEY);

                SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(secretKey);

                TokenValidationParameters parameters = new TokenValidationParameters {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = symmetricSecurityKey
                };

                ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, parameters, out _);

                return principal;

            } catch (Exception) {

                return null;
            }
        }


        public bool IsValidToken(string token) {

            ClaimsPrincipal principal = GetPrincipalFromToken(token);
            if (principal == null) {
                return false;
            }

            try {
                ClaimsIdentity identity = (ClaimsIdentity)principal.Identity;
                if (identity == null) {
                    return false;
                }
            } catch (NullReferenceException) {
                return false;
            }

            return true;
        }
    }
}