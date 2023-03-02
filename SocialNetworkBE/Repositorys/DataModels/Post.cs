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
        public DateTime CreateDate { get; set; }
        public DateTime UpdateAt { get; set; }
        public string Scope { get; set; }
        public string Content { get; set; }
        public List<string> Media { get; set; }
        public int NumofComment { get; set; }
        public IDictionary<string,Comment> Comments { get; set; }
        public IDictionary<string, Like> Likes { get; set; }
    }
}