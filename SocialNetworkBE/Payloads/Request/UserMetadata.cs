using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.JsonWebToken;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;

namespace SocialNetworkBE.Payloads.Request {
    public class UserMetadata {
        private JsonWebTokenService jsonWebTokenService;
        public string Id { get; set; }
        public string AvatarURL { get; set; }
        public string DisplayName { set; get; }
        public string UserProfileUrl { set; get; }
        public UserMetadata GetUserMetadataFromRequest(HttpRequestMessage request) {
            request.Properties
                .TryGetValue(
                    ConstantConfig.USER_META_DATA,
                    out var outUserMetadata
                );
            return outUserMetadata as UserMetadata;
        }
        public UserMetadata GetUserMetadataFromToken(string refreshToken)
        {
            var claimsIdentity = jsonWebTokenService.GetClaimsIdentityFromToken(refreshToken);
            var userId = jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(claimsIdentity, "UserId");
            var role = jsonWebTokenService.GetValueFromClaimIdentityByTypeClaim(claimsIdentity, "Role");
            return new UserMetadata() { Id = userId};
        }
    }
}