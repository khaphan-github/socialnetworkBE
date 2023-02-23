using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Services.Authenticate;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("auth")]

    public class AuthController : ApiController {
        
        [HttpPost]
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
    }
}