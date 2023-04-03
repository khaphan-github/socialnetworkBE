using ServiceStack;
using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Authenticate;
using SocialNetworkBE.Services.Notification;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace SocialNetworkBE.Controllers {
    public class AuthController : ApiController {

        private const string REFIX = "api/v1/auth";
        private readonly AuthService authService = new AuthService();
        private PushNotificationService _pushNotificationService;

        [HttpPost]
        [Route(REFIX + "/")]
        public async Task<ResponseBase> SignIn([FromBody] Auth authRequest) {

            bool isEmptyParams = 
                string.IsNullOrWhiteSpace(authRequest.Username) || 
                string.IsNullOrWhiteSpace(authRequest.Password);

            if (isEmptyParams) {
                return new ResponseBase() {
                    Message = "Request missing Username or Password in request's body",
                    Status = Status.WrongFormat,
                };
            }

            bool isTooLongParamValue =
                    authRequest.Username.Length > 254 || authRequest.Password.Length > 254;

            if (isTooLongParamValue) {
                return new ResponseBase() {
                    Message = "Request param value too long, must be < 254 charactor",
                    Status = Status.WrongFormat,
                };
            }

            return await authService.HandleUserAuthenticate(authRequest);
        }

        [HttpPost]
        [Route(REFIX + "/signup")]
        public ResponseBase SignUp()
        {
            
            var pwd = FormData.GetValueByKey("Password");
            if (pwd == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Password required"
                };
            }

            var email = FormData.GetValueByKey("Email");
            if (email == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Email required"
                };
            }

            var userName = FormData.GetValueByKey("Username");
            if (userName == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Username required"
                };
            }


            var DisplayName = FormData.GetValueByKey("Displayname");
            if (DisplayName == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "DisplayName required"
                };
            }


            AuthService authService = new AuthService();
            return authService.HandleUserSignUp(userName, pwd, email, DisplayName);

        }

        [HttpPost]
        [Route(REFIX + "/token")]
        public async Task<ResponseBase> RefreshToken([FromBody] Token tokenRequest) {
            bool isEmptyParams = 
                string.IsNullOrWhiteSpace(tokenRequest.AccessToken) || 
                string.IsNullOrWhiteSpace(tokenRequest.RefreshToken);
            
            if (isEmptyParams) {
                return new ResponseBase() {
                    Message = "Request missing AccessToken or RefreshToken in request's body",
                    Status = Status.WrongFormat,
                };
            }
            

            return authService.HandleRefreshTokenAsync(tokenRequest);
        }
        
    }
}