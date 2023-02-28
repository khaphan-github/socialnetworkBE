using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Services.JsonWebToken;
using System.Collections.Generic;
using System.Security.Claims;

namespace SocialNetworkBE.Services.Authenticate {
    public class AuthService {

        public AuthResponse HandleUserAuthenticate(Auth auth) {
            // Get user from db
            
            // create token
            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
            ClaimsIdentity claims =
                jsonWebTokenService.CreateClaimsIdentity("username", "email", "role");

            List<string> tokenKeyPairs = jsonWebTokenService.GenerateKeyPairs(claims);

            return new AuthResponse(
                     tokenKeyPairs[0],
                     tokenKeyPairs[1],
                     "User:id",
                     "username",
                     "userAvataURL"
                );
        }

        public TokenResponse HandleRefreshToken(Token tokenRequest) {

            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
            List<string> tokenKeyPair =
                jsonWebTokenService.RefreshToken(tokenRequest.AccessToken, tokenRequest.RefreshToken);

            if (tokenKeyPair.Count != 0) {
                TokenResponse tokenResponse = new TokenResponse() {
                    AccessToken = tokenKeyPair[0],
                    RefreshToken = tokenKeyPair[1]
                };
                return tokenResponse;
            }

            return null;
        }

        public object HandleUserSignup() {
            return null;
        }
    }
}