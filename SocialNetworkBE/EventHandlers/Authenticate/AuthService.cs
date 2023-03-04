using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;
using SocialNetworkBE.Services.JsonWebToken;
using System.Collections.Generic;
using System.Security.Claims;

namespace SocialNetworkBE.Services.Authenticate {
    public class AuthService {

        public ResponseBase HandleUserAuthenticate(Auth auth) {

            AccountResponsitory accountResponsitory = new AccountResponsitory();

            Account savedAccount =
                accountResponsitory.GetAccountByUsername(auth.Username);

            if (savedAccount == null) {
                return new ResponseBase() {
                    Status = Status.Unauthorized,
                    Message = "User: " + auth.Username +" is not exist",
                };
            }

            BCryptService bCryptService = new BCryptService();

            string secretKey = ServerEnvironment.GetServerSecretKey();

            string verifyText = 
                bCryptService.GetHashCode(savedAccount.HashSalt, auth.Password, secretKey);

            bool isValidated =
                bCryptService.ValidateStringAndHashBySHA512(verifyText, savedAccount.Password);
           
            if (!isValidated) {
                return new ResponseBase() {
                    Status = Status.Unauthorized,
                    Message = "Username or password was wrong",
                };
            }

            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();

            ClaimsIdentity claims =
                jsonWebTokenService
                .CreateClaimsIdentity(savedAccount.Username, savedAccount.Email, "user");

            List<string> tokenKeyPairs = jsonWebTokenService.GenerateKeyPairs(claims);

            AuthResponse authResponse =
                new AuthResponse(
                     tokenKeyPairs[0],
                     tokenKeyPairs[1],
                     savedAccount.Id.ToString(),
                     savedAccount.DisplayName,
                     savedAccount.AvatarUrl,
                     savedAccount.UserProfileUrl
                );

            ResponseBase response =
                new ResponseBase() {
                    Status = Status.Success,
                    Message = "Authorized",
                    Data = authResponse
                };

            return response;
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