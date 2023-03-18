using System.Linq;
using System.Web.Http;

namespace SocialNetworkBE {
    public class WebApiApplication : System.Web.HttpApplication {
        protected void Application_Start() {
  
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
