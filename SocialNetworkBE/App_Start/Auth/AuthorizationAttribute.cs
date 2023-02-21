using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Services.JsonWebToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SocialNetworkBE.App_Start.Auth {
    public class AuthorizationAttribute : AuthorizationFilterAttribute {
        public override void OnAuthorization(HttpActionContext actionContext) {
            AuthenticationHeaderValue authHeaderValue = actionContext.Request.Headers.Authorization;

            bool isAuthRequestHeaderEmpty = authHeaderValue == null;

            if (isAuthRequestHeaderEmpty) {
                string[] pargam = { "Authorization" };

                ResponseBase responseEmptyRequest = new ResponseBase().EmptyRequestDataResponse(pargam);

                actionContext.Response =
                    actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseEmptyRequest);

                return;
            }

            string accessToken = authHeaderValue.Parameter;

            JsonWebTokenService jsonWebTokenService = new JsonWebTokenService();
            bool isValidAccessToken = jsonWebTokenService.IsValidToken(accessToken);

            if (!isValidAccessToken) {
                ResponseBase responseBase = new ResponseBase() {
                    Message = "Invalid Token",
                    Status = Status.Failure
                };

                actionContext.Response = actionContext.Request
                       .CreateResponse(HttpStatusCode.Unauthorized, responseBase);
            }
        }
    }
}