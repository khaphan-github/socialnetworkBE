using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Repositorys.DataModels
{
    public class InviteFriendNotify
    {
        public string Type { get; set; } = "Invitation";
        public string Id { get; set; }
        
        public string AvatarUserSentUrl { get; set; }
        public string DisplaynameUserSent { get; set; }
        public string onDeleteHref { get; set; }
        public string onMarkHref { get; set; }
        public string inviteHref { get; set; }
        public string acceptHref { get; set; }
        public string denyHref { get; set; }

    }
}