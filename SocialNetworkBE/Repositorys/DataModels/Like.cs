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
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerProfileURL { get; set; }
        public string TypeofAction { set; get; } = "like";
        public DateTime CreateDate { set; get; } = DateTime.Now;
    }
}