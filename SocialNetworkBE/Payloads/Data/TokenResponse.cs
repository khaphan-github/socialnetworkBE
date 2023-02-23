using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Data {
    public class TokenResponse {
        public string AccessToken { get; set; }
        public string RefeshToken { get; set; }
        public int Exprise { get; set; }
        public string RefreshTokenURL { get; set; }
    }
}