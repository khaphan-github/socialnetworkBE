using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Services.Authenticate;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {

    public class AuthController : ApiController {
        private const string REFIX = "api/v1/auth";
        
        [HttpPost]
        [Route(REFIX + "/")]
        public ResponseBase SignIn([FromBody] Auth authRequest) {

            bool isEmptyParams = authRequest.Username == null || authRequest.Password == null;

            if (isEmptyParams) {
                return new ResponseBase() {
                       Message = "Request missing Username or Password in request's body",
                       Status = Status.WrongFormat,
                };
            }

            bool isTooLongParamValue = authRequest.Username.Length > 254 || authRequest.Password.Length > 254;

            if (isTooLongParamValue) {
                return new ResponseBase() {
                    Message = "Request param value too long, must be < 254 charactor",
                    Status = Status.WrongFormat,
                };
            }

            AuthService authService = new AuthService();

            ResponseBase response = new ResponseBase() {
                Status = Status.Success,
                Message = "Success",
                Data = authService.HandleUserAuthenticate(authRequest)
            };

            return response;
        }

        [HttpPost]
        [Route(REFIX + "/token")]
        public ResponseBase RefreshToken([FromBody]Token tokenRequest) {
            bool isEmptyParams = tokenRequest.AccessToken == null || tokenRequest.RefreshToken == null;

            if (isEmptyParams) {
                return new ResponseBase() {
                    Message = "Request missing AccessToken or RefreshToken in request's body",
                    Status = Status.WrongFormat,
                };
            }


            AuthService authService = new AuthService();

            TokenResponse tokenResponse = authService.HandleRefreshToken(tokenRequest);

            if (tokenResponse== null) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Token invalid"
                };
            }

            ResponseBase response = new ResponseBase() {
                Status = Status.Success,
                Message = "Success",
                Data = tokenResponse
            };

            return response;
        }
    }
}