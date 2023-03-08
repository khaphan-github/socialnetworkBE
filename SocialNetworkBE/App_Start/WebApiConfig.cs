using SocialNetworkBE.App_Start.Auth;
using SocialNetworkBE.ServerConfiguration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SocialNetworkBE {
    public static class WebApiConfig {
        private const string ApiEndpoint = "api/v1/{controller}/{id}";
        public static void Register(HttpConfiguration config) {

            // Web API configuration and services
            var cors = new EnableCorsAttribute(ServerEnvironment.GetClientCORSDomain(), "*", "*");
            config.EnableCors(cors);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: ApiEndpoint,
                defaults: new { id = RouteParameter.Optional }
            );

            // Web Api Add new Authentication
            config.Filters.Add(new AuthorizationAttribute());
        }
    }
}
