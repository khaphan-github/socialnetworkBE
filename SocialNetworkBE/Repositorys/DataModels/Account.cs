using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class Account
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        [Unique]
        public string Username { get; set; }
        public string Password { get; set; }
        public string HashSalt { get; set; }    
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string DisplayName { set; get; }
        public string AvatarUrl { set; get; }
        [Unique]
        public string UserProfileUrl { set; get; }
        public int NumberOfFriend { get; set; } = 0;
        public List<ObjectId> ListFriendsObjectId{get; set;}
        public int NumberOfFriendPost { get; set; } = 0;
        public List<ObjectId> ListPostsObjectId { get; set; }
    }
}
