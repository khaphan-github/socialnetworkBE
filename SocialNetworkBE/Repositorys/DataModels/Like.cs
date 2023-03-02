using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class Like
    {
        public ObjectId OwnerId { set; get; }
        public string TypeofAction { set; get; }
        public DateTime CreateDate { set; get; }
    }
}