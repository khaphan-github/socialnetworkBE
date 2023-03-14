using MongoDB.Bson;
using System;

namespace SocialNetworkBE.Payloads.Response {
    public class CommentResponse {

        public ObjectId CommentId { get; set; }
        public ObjectId OwnerId { get; set; }
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerProfileURL { get; set; }
        public string Content { get; set; }
        public string ChildrenHref { get; set; }
        public DateTime CreateDate { get; set; } 
        public int NumOfComment { get; set; }
        public int NumOfLike { get; set; }
    }
}