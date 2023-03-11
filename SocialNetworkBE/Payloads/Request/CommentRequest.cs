using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Payloads.Request {
    public class CommentRequest {
        public ObjectId PostId { get; set; }
        public ObjectId OwnerId { get; set; }
        public string Comment { get; set; }
    }
}