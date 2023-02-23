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
            int exprise,
            string refreshTokenURL,
            string userId,
            string username,
            string avatarURL) {

            Token = new TokenResponse() {
                AccessToken = accessToken,
                RefeshToken = refreshToken,
                Exprise = exprise,
                RefreshTokenURL = refreshTokenURL,
            };

            User = new UserResponse() {
                Id = userId,
                Username = username,
                AvatarURL = avatarURL,
            };

        }
    }
}