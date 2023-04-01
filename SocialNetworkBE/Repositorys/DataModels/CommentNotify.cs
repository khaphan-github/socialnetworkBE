using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class CommentNotify
    {
        public string Type { get; set; } = "Comment";
        public string Id { get; set; }

        public string AvatarUserCommentUrl { get; set; }
        public string DisplaynameUserComment { get; set; }
        public string onDeleteHref { get; set; }
        public string onMarkHref { get; set; }
        public string commentHref { get; set; }

        public string postHref { get; set; }
    }
}