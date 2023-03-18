﻿using SocialNetworkBE.Payload.Request;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Services.Authenticate;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    public class AuthController : ApiController {

        private const string REFIX = "api/v1/auth";
        private readonly AuthService authService = new AuthService();

        [HttpPost]
        [Route(REFIX + "/")]
        public ResponseBase SignIn([FromBody] Auth authRequest) {

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

            return authService.HandleUserAuthenticate(authRequest);
        }

        [HttpPost]
        [Route(REFIX + "/signup")]
        public ResponseBase SignUp()
        {
            
            var pwd = FormData.GetValueByKey("pwd");
            if (pwd == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Password required"
                };
            }

            var email = FormData.GetValueByKey("email");
            if (email == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Email required"
                };
            }

            var userName = FormData.GetValueByKey("userName");
            if (userName == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Username required"
                };
            }


            var DisplayName = FormData.GetValueByKey("DisplayName");
            if (DisplayName == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "DisplayName required"
                };
            }

            var AvatarUrl = FormData.GetValueByKey("AvatarUrl");
            if (AvatarUrl == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "AvatarUrl required"
                };
            }

            var UserProfileUrl = FormData.GetValueByKey("UserProfileUrl");
            if (UserProfileUrl == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "AvatarUrl required"
                };
            }
            return authService.HandleUserSignUp(userName, pwd, email, DisplayName, AvatarUrl, UserProfileUrl);
        }

        [HttpPost]
        [Route(REFIX + "/token")]
        public ResponseBase RefreshToken([FromBody] Token tokenRequest) {
            bool isEmptyParams = 
                string.IsNullOrWhiteSpace(tokenRequest.AccessToken) || 
                string.IsNullOrWhiteSpace(tokenRequest.RefreshToken);
            
            if (isEmptyParams) {
                return new ResponseBase() {
                    Message = "Request missing AccessToken or RefreshToken in request's body",
                    Status = Status.WrongFormat,
                };
            }

            return authService.HandleRefreshToken(tokenRequest);
        }
    }
}