using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class Comment
    {
        public ObjectId OwnerId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.Now;
        public List<Comment> Comments { get; set; }

        public int NumofLike { get; set; } = 0;
        public List<Like> Likes { set; get; }
    }
}
