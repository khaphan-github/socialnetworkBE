using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Repositorys.DataModels {
    public class Comment {
        public ObjectId Id { get; set; }
        public ObjectId PostId { get; set; }
        public ObjectId? ParentId { get; set; }
        public ObjectId OwnerId { get; set; }
        public string Content { get; set; }
        public int CommentCount { get; set; } = 0;
        public int ActionCount { get; set; } = 0;
        public List<ObjectId> ActionUser { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
