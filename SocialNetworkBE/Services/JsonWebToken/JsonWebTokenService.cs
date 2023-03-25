using Microsoft.IdentityModel.Tokens;
using SocialNetworkBE.ServerConfiguration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetworkBE.Services.JsonWebToken {
    public class JsonWebTokenService {
        // https://gist.github.com/rafiulgits/b967c3f1716436b3c59d038d04a51009
        
        private readonly string SECRET_KEY = ServerEnvironment.GetServerSecretKey();
        
        public readonly int accessTokenExpriseTime = 10; // 10 Minutes
        public readonly int refreshTokenExpriseTime = 5 * 24 * 60; // 5 Days

        public readonly string AccessToken = "AccessToken";
        public readonly string RefreshToken = "RefreshToken";
        public readonly string KeyClaimsToken = "token_type";
        
        public ClaimsIdentity CreateClaimsIdentityFromModel(TokenClaimsModel tokenClaimsModel, string tokenType) {
            if (tokenClaimsModel == null) throw new ArgumentNullException("tokenClaimsModel");
            ClaimsIdentity claimsIdentity = new ClaimsIdentity();

            claimsIdentity.AddClaim(new Claim(ClaimTypes.Email, tokenClaimsModel.Email));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, tokenClaimsModel.DisplayName));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, tokenClaimsModel.Role));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, tokenClaimsModel.ObjectId));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, tokenClaimsModel.Username));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Webpage, tokenClaimsModel.UserProfileURL));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.UserData, tokenClaimsModel.AvatarURL));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Hash, tokenClaimsModel.KeyPairHash));
            claimsIdentity.AddClaim(new Claim(KeyClaimsToken, tokenType));

            return claimsIdentity;
        }

        public string CreateTokenFromClaims(ClaimsIdentity claimsIdentity, int exprisesTime) {

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
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(descriptor);

            return handler.WriteToken(securityToken);
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

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public ClaimsIdentity GetClaimsIdentityFromToken(string token) {
            ClaimsPrincipal claimsPrinclipal = GetPrincipalFromToken(token);
            if (claimsPrinclipal != null) {
                try {
                    return (ClaimsIdentity)claimsPrinclipal.Identity;
                } catch (NullReferenceException ex) {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            return null;
        }

        /** @param: claimType must be ClaimTypes*/
        public string GetValueFromClaimIdentityByTypeClaim(ClaimsIdentity claimsIdentity, string claimType) {
            try {
                if (claimsIdentity == null) return "";
                return claimsIdentity.FindFirst(claimType).Value;

            } catch (ArgumentNullException) {
                return "";
            }
        }
    }
}