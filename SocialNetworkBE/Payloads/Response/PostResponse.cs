using MongoDB.Bson;
using System.Collections.Generic;
using System;

namespace SocialNetworkBE.Payloads.Response {
    public class PostResponse {
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerProfileURL { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public string Scope { get; set; } = "public";
        public string Content { get; set; }
        public List<string> Media { get; set; }
        public int NumOfComment { get; set; } = 0;
        public string CommentsURL { get; set; }
        public int NumOfLike { get; set; } = 0;
        public string LikeURL { get; set;}
    }
}