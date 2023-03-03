using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Data {
    public class TokenResponse {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int Exprise { get; set; } = 10;
        public string RefreshTokenURL { get; set; } = "api/v1/auth/token";
    }
}