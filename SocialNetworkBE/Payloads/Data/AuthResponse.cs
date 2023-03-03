using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Data {
    public class AuthResponse {
        public TokenResponse Token { get; set; }
        public UserResponse User { get; set; }

        public AuthResponse(
            string accessToken,
            string refreshToken,
            string userId,
            string displayName,
            string avatarURL,
            string userProfileUrl) {

            Token = new TokenResponse() {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            User = new UserResponse() {
                Id = userId,
                DisplayName = displayName,
                AvatarURL = avatarURL,
                UserProfileUrl= userProfileUrl
            };

        }
    }
}