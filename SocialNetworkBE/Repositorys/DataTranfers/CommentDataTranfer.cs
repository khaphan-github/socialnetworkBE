using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Repositorys.DataTranfers {
    public class CommentDataTranfer {
        public ObjectId Id { get; set; }
        public ObjectId PostId { get; set; }
        public ObjectId? ParentId { get; set; }
        public ObjectId OwnerId { get; set; }
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerProfileURL { get; set; }
        public string Content { get; set; }
        public int CommentCount { get; set; }
        public int ActionCount { get; set; }
        public List<ObjectId> ActionUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}