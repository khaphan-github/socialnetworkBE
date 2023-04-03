using Microsoft.IdentityModel.Tokens;
using ServiceStack;
using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Authenticate;
using SocialNetworkBE.Services.JsonWebToken;
using SocialNetworkBE.Services.Notification;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using RouteAttribute = System.Web.Http.RouteAttribute;

namespace SocialNetworkBE.Controllers {
    public class AuthController : ApiController {

        private const string REFIX = "api/v1/auth";
        private readonly AuthService authService = new AuthService();
        private PushNotificationService _pushNotificationService;
        private readonly JsonWebTokenService jsonWebTokenService;

        public AuthController()
        {
            jsonWebTokenService = new JsonWebTokenService();
        }

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
        public async Task<ResponseBase> RefreshToken([FromBody] Token tokenRequest)
        {
            bool isEmptyParams =
                string.IsNullOrWhiteSpace(tokenRequest.AccessToken) ||
                string.IsNullOrWhiteSpace(tokenRequest.RefreshToken);

            if (isEmptyParams)
            {
                return new ResponseBase()
                {
                    Message = "Request missing AccessToken or RefreshToken in request's body",
                    Status = Status.WrongFormat,
                };
            }
            var tokenResponse = authService.HandleRefreshTokenAsync(tokenRequest);
            ClaimsIdentity validAccessToken = jsonWebTokenService.GetClaimsIdentityFromToken(tokenRequest.AccessToken);
            if (validAccessToken == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Unauthorized,
                    Message = "Access token is invalid or expired."
                };
            }

            // Lấy access token mới từ đối tượng TokenResponse
            bool isAccessTokenType = jsonWebTokenService
            .GetValueFromClaimIdentityByTypeClaim(validAccessToken, jsonWebTokenService.KeyClaimsToken)
            .Equals(jsonWebTokenService.AccessToken);
            if (!isAccessTokenType)
            {
                return new ResponseBase()
                {
                    Status = Status.Unauthorized,
                    Message = "Token in AccessToken param is not an access token"
                };
            }

            // Get user metadata
            string refreshToken = tokenRequest.RefreshToken;
            UserMetadata userMetadata = new UserMetadata().GetUserMetadataFromToken(refreshToken);
            var userId = userMetadata.Id;
            Debug.WriteLine(userId);
            
            _pushNotificationService = new PushNotificationService(userId, tokenRequest.AccessToken);
            await _pushNotificationService.SendAccessTokenToNotifiService();
            
            // Gọi phương thức HandleRefreshTokenAsync để làm mới access token và trả về response chứa token mới
            
            return tokenResponse;
        }


    }
}