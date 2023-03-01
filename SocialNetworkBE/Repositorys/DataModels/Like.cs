using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class Like
    {
        public ObjectId Id { set; get; }
        public string TypeofHeart { set; get; }
        public DateTime CreateDate { set; get; }
    }
}