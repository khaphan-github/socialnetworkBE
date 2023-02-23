﻿using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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

        // TODO: Handle Secure Secretkey using OpenSSL of anything same;

        private const string SECRET_KEY = "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh" +
            "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh" +
            "hhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh";


        public ClaimsIdentity CreateClaimsIdentity(string username, string email, string role) {
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

                ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, parameters, out _);

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
            string accessToken = CreateTokenFromUserData(claimsIdentity, 5);

            string refreshToken = CreateTokenFromUserData(claimsIdentity, 15);

            List<string> keyPairs = new List<string>() {
                accessToken, refreshToken
            };
            return keyPairs;
        }

        public List<string> RefreshToken(string accessToken, string refreshToken) {
         /*   
          *   if(IsValidToken(accessToken)) {
                return new List<string>();
            }
         */
            bool isValidRefreshToken = IsValidToken(accessToken);
            List<string> keyPairs = new List<string>();

            if (isValidRefreshToken) {
                ClaimsIdentity claimsIdentity = GetClaimsIdentityFromToken(refreshToken);
                keyPairs =  GenerateKeyPairs(claimsIdentity);
            }

            return keyPairs;
        }
    }
}