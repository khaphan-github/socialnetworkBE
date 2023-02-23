using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Services.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Web;

namespace SocialNetworkBE.Services.Authenticate {
    public class AuthService {

        public AuthResponse HandleUserAuthenticate(Auth auth) {


            // Get user from db

            // create token
            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
            ClaimsIdentity claims = jsonWebTokenService.CreateClaimsIdentity("username", "email", "role");

            List<string> tokenKeyPairs = jsonWebTokenService.GenerateKeyPairs(claims);

            return new AuthResponse(
                     tokenKeyPairs[0],
                     tokenKeyPairs[1],
                     5,
                     "/api/v1/auth/token",
                     "User:id",
                     "username",
                     "userAvataURL"
                );
        }

        public object handleUserSignup() {
            return null;
        }

    }
}