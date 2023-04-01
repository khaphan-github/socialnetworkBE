using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Request
{
    public class AccountRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string HashSalt { get; set; }
        public string Email { get; set; }
        public string DisplayName { set; get; }
        public string AvatarUrl { set; get; }
    }
}