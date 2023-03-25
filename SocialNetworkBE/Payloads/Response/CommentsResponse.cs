using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Payloads.Response {
    public class CommentsResponse {
        public ObjectId Id { get; set; }
        public ObjectId PostId { get; set; }
        public ObjectId? ParentId { get; set; }
        public ObjectId OwnerId { get; set; }
        public bool IsHaveChild { get; set; }
        public int ChildCount { get; set; }
        public string ChildrenHref { get; set; }
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerProfileURL { get; set; }
        public string Content { get; set; }
        public int ActionCount { get; set; }
        public List<ObjectId> ActionUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}