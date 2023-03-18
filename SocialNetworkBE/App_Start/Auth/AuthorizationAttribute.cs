using ServiceStack;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.JsonWebToken;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SocialNetworkBE.App_Start.Auth {
    public class AuthorizationAttribute : AuthorizationFilterAttribute {

        private readonly string[] NoAuthorizationURL = {
            "/api/v1/auth"
        };
        public override void OnAuthorization(HttpActionContext actionContext) {
            string requestUrl = actionContext.Request.RequestUri.ToString();

            bool isPassAuthFilter = false;

            for (int i = 0; i < NoAuthorizationURL.Length; i++) {
                bool isMatchUrl = requestUrl.Contains(NoAuthorizationURL[i]);
                if (isMatchUrl) {
                    isPassAuthFilter = true;
                    break;
                }
            }

            if (!isPassAuthFilter) {
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
                ClaimsIdentity validAccessToken = jsonWebTokenService.GetClaimsIdentityFromToken(accessToken);

                if (validAccessToken == null) {
                    ResponseBase responseBase = new ResponseBase() {
                        Message = "Invalid token",
                        Status = Status.Unauthorized
                    };

                    actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseBase);
                }

                string tokenType =
                    jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(validAccessToken, jsonWebTokenService.KeyClaimsToken);

                if (tokenType == jsonWebTokenService.RefreshToken) {
                    ResponseBase responseBase = new ResponseBase() {
                        Message = "Refresh token only use to get new token keypair - not use to authorize",
                        Status = Status.Unauthorized
                    };

                    actionContext.Response =
                        actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseBase);
                }

                // Todo: get claims from token then pass them to controller:

                actionContext.Request.Properties
                    .Add(new KeyValuePair<string, object>(
                        ConstantConfig.USER_META_DATA, 
                        new UserMetadata() {
                            Id = jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(validAccessToken, ClaimTypes.Sid),
                            DisplayName = jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(validAccessToken, ClaimTypes.Name),
                            AvatarURL = jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(validAccessToken, ClaimTypes.UserData),
                            UserProfileUrl = jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(validAccessToken, ClaimTypes.Webpage),
                    }));
            }
        }
    }
}