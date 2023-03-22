
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Response
{
    public class FriendRespone
    {
        public ObjectId Id { get; set; }
        public string DisplayName { get; set; }
        public string Avatar { get; set; }
        public string ProfileUrl { get; set; }
    }
}