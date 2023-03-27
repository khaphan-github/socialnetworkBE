using MongoDB.Bson;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Data
{
    public class AccountRespone
    {
        public ObjectId Id { get; set; }
        [Unique]
        public string Username { get; set; }
        public string Email { get; set; }
        public string DisplayName { set; get; }
        public string AvatarUrl { set; get; }
        [Unique]
        public string UserProfileUrl { set; get; }
    }
}