using SocialNetworkBE.ServerConfiguration;
using System.Net.Http;

namespace SocialNetworkBE.Payloads.Request {
    public class UserMetadata {
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
    }
}