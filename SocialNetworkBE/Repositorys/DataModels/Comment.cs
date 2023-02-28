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
        public string name { get; set; }
        public string email { get; set; }
        public ObjectId movie_id { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }
    }
}
