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
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Object>Comments { get; set; }

        public int NumofLike { get; set; }
        public List<ObjectId> Likes { set; get; }
    }
}
