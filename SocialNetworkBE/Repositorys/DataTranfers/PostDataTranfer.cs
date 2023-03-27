using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Repositorys.DataTranfers {
    public class PostDataTranfer {
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
        public string LikesURL { get; set; }
        public bool IsLiked { get; set; }
    }
}