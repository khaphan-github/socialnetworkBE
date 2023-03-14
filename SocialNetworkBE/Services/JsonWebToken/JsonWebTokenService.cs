using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SocialNetworkBE.ServerConfiguration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;

namespace SocialNetworkBE.Services.JsonWebToken {
    public class JsonWebTokenService {
        // https://gist.github.com/rafiulgits/b967c3f1716436b3c59d038d04a51009
        private readonly string SECRET_KEY = ServerEnvironment.GetServerSecretKey();

        public ClaimsIdentity CreateClaimsIdentity(string username, string email, string role) {
            if (username== null) throw new ArgumentNullException("username");
            if (email== null) throw new ArgumentNullException("email");
            if (role== null) throw new ArgumentNullException("role");

            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, username));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, role));

            return claimsIdentity;
        }

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

        private ClaimsPrincipal GetPrincipalFromToken(string token) {
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

                ClaimsPrincipal principal =
                    jwtSecurityTokenHandler.ValidateToken(token, parameters, out _);

                return principal;

            } catch (Exception) {
                return null;
            }
        }

        public bool IsValidToken(string token) {
            ClaimsIdentity claimsIdentity = GetClaimsIdentityFromToken(token);
            if (claimsIdentity == null) {
                return false;
            }
            return true;
        }

        private ClaimsIdentity GetClaimsIdentityFromToken(string token) {
            ClaimsPrincipal claimsPrinclipal = GetPrincipalFromToken(token);

            if (claimsPrinclipal == null) {
                return null;
            }

            try {
                ClaimsIdentity identity = (ClaimsIdentity)claimsPrinclipal.Identity;
                if (identity == null) {
                    return null;
                }
                return identity;
            } catch (NullReferenceException) {
                return null;
            }

        }

        public List<string> GenerateKeyPairs(ClaimsIdentity claimsIdentity) {

            int accessTokenExpriseTime = 10; // 10 Minutes
            int refreshTokenExpriseTime = 5 * 24 * 60; // 5 Days

            string accessToken =
                CreateTokenFromUserData(claimsIdentity, accessTokenExpriseTime);

            string refreshToken =
                CreateTokenFromUserData(claimsIdentity, refreshTokenExpriseTime);

            List<string> keyPairs = new List<string>() {
                accessToken, refreshToken
            };

            return keyPairs;
        }

        /** 
         Only access refresh token when accesstoken invalid 
        */
        public List<string> RefreshToken(string accessToken, string refreshToken) {
            bool isValidAccessToken = IsValidToken(accessToken);

            if (isValidAccessToken) {
                return new List<string>();
            }
            ClaimsIdentity claimsIdentity = GetClaimsIdentityFromToken(refreshToken);
            return GenerateKeyPairs(claimsIdentity);
        }
    }
}