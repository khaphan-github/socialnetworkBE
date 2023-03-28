using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(SocialNetworkBE.App_Start.Startup))]
namespace SocialNetworkBE.App_Start {
    public class Startup {
        const string SignalRServerURL = "/notification-server";
        const string SignalRChatServerURL = "/notification-chanel";

        public void Configuration(IAppBuilder app) {
            app.Map(SignalRServerURL, map => {
                var hubConfiguration = new HubConfiguration {

                };
                map.RunSignalR(hubConfiguration);
            });

            app.Map(SignalRChatServerURL, map => {
                var hubConfiguration = new HubConfiguration {

                };
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}
