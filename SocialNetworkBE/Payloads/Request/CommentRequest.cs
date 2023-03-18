using MongoDB.Bson;

namespace SocialNetworkBE.Payloads.Request {
    public class CommentRequest {
        public string PostId { get; set; }
        public string CommentId { get; set; }
        public string Comment { get; set; }
    }
}