using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class LikeNotify
    {
        public string Type { get; set; } = "Like";
        public string Id { get; set; }

        public string AvatarUserCommentUrl { get; set; }
        public string DisplaynameUserComment { get; set; }
        public string onDeleteHref { get; set; }
        public string onMarkHref { get; set; }
        public string postHref { get; set; }
    }
}