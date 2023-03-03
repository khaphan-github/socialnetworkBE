using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Data {
    public class UserResponse {
        public string Id { get; set; }
        public string AvatarURL { get; set; }
        public string DisplayName { set; get; }
        public string UserProfileUrl { set; get; }
    }
}