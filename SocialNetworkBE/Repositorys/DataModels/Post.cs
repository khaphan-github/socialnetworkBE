using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using MongoDB.Bson;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class Post
    {
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }    
        public string OwnerProfileURL { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public string Scope { get; set; } = "public";
        public string Content { get; set; }
        public List<string> Media { get; set; }
        public int NumOfComment { get; set; } = 0;
        public string CommentsURL { get; set; }

        public List<Comment> Comments { get; set; }
        public int NumOfLike { get; set; } = 0;
        public string LikesURL { get; set; }
        public List<Like> Likes { get; set; }
    }
}