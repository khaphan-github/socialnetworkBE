using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Services.JsonWebToken;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SocialNetworkBE.App_Start.Auth {
    public class AuthorizationAttribute : AuthorizationFilterAttribute {

        private readonly string[] NoAuthorizationURL = {
            "/api/v1/auth"
        };
        public override void OnAuthorization(HttpActionContext actionContext) {
            string requestUrl = actionContext.Request.RequestUri.ToString();

            bool isPass = false;

            for (int i = 0; i < NoAuthorizationURL.Length; i++) {
                bool isMatchUrl = requestUrl.Contains(NoAuthorizationURL[i]);
                if (isMatchUrl) {
                    isPass = true;
                    break;
                }
            }

            if (!isPass) {
                AuthenticationHeaderValue authHeaderValue = actionContext.Request.Headers.Authorization;

                bool isAuthRequestHeaderEmpty = authHeaderValue == null;

                if (isAuthRequestHeaderEmpty) {

                    ResponseBase responseEmptyRequest = new ResponseBase() {
                        Message = "Missing Authorization in request's header",
                        Status = Status.Unauthorized
                    };

                    actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseEmptyRequest);

                    return;
                }

                string accessToken = authHeaderValue.ToString();

                JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
                bool isValidAccessToken = jsonWebTokenService.IsValidToken(accessToken);

                if (!isValidAccessToken) {
                    ResponseBase responseBase = new ResponseBase() {
                        Message = "Invalid token",
                        Status = Status.Failure
                    };

                    actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseBase);
                }
            }
        }
    }
}