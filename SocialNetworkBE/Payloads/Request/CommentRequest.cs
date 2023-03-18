using MongoDB.Bson;

namespace SocialNetworkBE.Payloads.Request {
    public class CommentRequest {
        public ObjectId PostId { get; set; }
        public ObjectId? CommentId { get; set; }
        public string Comment { get; set; }
    }
}