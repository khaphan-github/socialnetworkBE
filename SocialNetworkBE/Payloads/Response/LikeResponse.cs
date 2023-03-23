
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetworkBE.Payloads.Response {
    public class LikeResponse {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UserProfileUrl { get; set; }
        public string AvatarUrl { get; set; }
    }
}