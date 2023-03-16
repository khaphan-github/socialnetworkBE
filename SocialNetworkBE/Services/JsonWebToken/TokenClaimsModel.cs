using System;

namespace SocialNetworkBE.Services.JsonWebToken {
    public class TokenClaimsModel {
        public string Role { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ObjectId { get; set; }
        public string AvatarURL { get; set; }
        public string UserProfileURL { get; set; }
        public string KeyPairHash { get; set; } // GUID
    }
}