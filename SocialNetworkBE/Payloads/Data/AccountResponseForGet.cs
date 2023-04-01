using MongoDB.Bson;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Data
{
    public class AccountResponseForGet
    {
        public ObjectId Id { get; set; }
        [Unique]
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { set; get; }
        public string AvatarUrl { set; get; }
        [Unique]
        public string UserProfileUrl { set; get; }
        public int NumberOfFriend { get; set; } = 0;
        public List<ObjectId> ListFriendsObjectId { get; set; }
        public List<ObjectId> ListPostsObjectId { get; set; }
        public List<ObjectId> ListObjectId_UserSendInvite { get; set; }
        public List<ObjectId> ListObjectId_GiveUserInvitation { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}