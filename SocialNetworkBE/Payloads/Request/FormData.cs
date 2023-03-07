using System.Web;

namespace SocialNetworkBE.Payloads.Request {
    public static class FormData {
        public static string GetValueByKey(string key) {
            return HttpContext.Current.Request.Params[key];
        }
    }
}