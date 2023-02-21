using SocialNetworkBE.App_Start.Auth;
using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using System.Web.Http;
using System.Web.Http.Filters;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("auth")]
    public class AuthController : ApiController {
        [HttpPost]

        public ResponseBase SignIn([FromBody] Auth authenticateRequest) {

            if (authenticateRequest.Credential == null) {
                string[] pagrams = { "Credential" };

                return new ResponseBase().EmptyRequestDataResponse(pagrams);
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