using Microsoft.Owin.Cors;
using Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Cors;
using System.Web.Http.Cors;

namespace SocialNetworkBE.App_Start {
    public static class SignalRConfig {
        public static void Register(IAppBuilder app, EnableCorsAttribute cors) {
            app.Map("/signalr", map => {
                var corsOption = new CorsOptions {
                    PolicyProvider = new CorsPolicyProvider {
                        PolicyResolver = context => {
                            var policy = new CorsPolicy { 
                                AllowAnyHeader = true, 
                                AllowAnyMethod = true, 
                                SupportsCredentials = true };

                            policy.Origins.Add("http://localhost:3000");
                            return Task.FromResult(policy);
                        }
                    }
                };
                map.UseCors(corsOption).RunSignalR();
            });
        }
    }
}