using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("auth")]
    public class AuthController : ApiController {
        [HttpPost]
        public ResponseBase SignIn([FromBody] Auth authenticateRequest) {
            bool isEmptyRequestPagrams =
                authenticateRequest.Username == null || authenticateRequest.Password == null;

            if (isEmptyRequestPagrams) {
                string[] pagrams = { "username", "password" };

                return new ResponseBase().EmptyRequestBodyResponse(pagrams);
            }

            // TODO: Hanlde user sign in then get access token

            ResponseBase response = new ResponseBase() {
                Status = Status.Success,
                Message = "Success",
                Data = authenticateRequest
            };

            return response;
        }
    }
}