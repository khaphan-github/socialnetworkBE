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
    public class Account
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayName { set; get; }
        public string AvatarUrl { set; get; }


        public string UserProfileUrl { set; get; }
        public int NumofFriend { get; set; }
        public Array ListFriend{get; set;}
        public Array ListPost { get; set; }
    }
}
