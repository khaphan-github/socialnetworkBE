using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payload.Request {
    public class Auth {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}