using LiteDB;
using MongoDB.Bson;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static ServiceStack.Diagnostics.Events;
using JsonSerializer = System.Text.Json.JsonSerializer;
using ObjectId = MongoDB.Bson.ObjectId;

namespace SocialNetworkBE.Services.Notification
{
    public class PushNotificationService
    {
        public string UserId { get; set; }
        public object Message { get; set; } // Object then stringtify

        public readonly string NotifiServiceEndpoint = "http://localhost:3002/api/v1/service/notify";

        public HttpClientService HttpService { get; set; }
        public PushNotificationService(string userId, object message)
        {
            UserId = userId;
            Message = message;
            HttpService = new HttpClientService();
        }


        public async Task<bool> PushMessage()
        {
            string contentFormat = "{{\"UserId\":\"{0}\",\"Message\":{1}}}";
            string messageJson = JsonSerializer.Serialize<object>(Message);

            string postContent = string.Format(contentFormat, new object[2] { UserId, messageJson });

            HttpResponseMessage httpResponse =
                await HttpService.PostAsync(NotifiServiceEndpoint, postContent);
            return httpResponse.IsSuccessStatusCode;
        }


        public async Task<bool> SendAccessTokenToNotifiService()
        {
            string contentFormat = "{{\"UserId\":\"{0}\",\"AccessToken\":\"{1}\"}}";
            string putContent = string.Format(contentFormat, new object[2] { UserId, Message });

            HttpResponseMessage httpResponse =
                await HttpService.PutAsync(NotifiServiceEndpoint, putContent);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> PushMessage_WhenReceiveInvitation(string userReceive)
        {
            AccountResponsitory accountResponsitory = new AccountResponsitory();
            AccountRespone accountRespone = accountResponsitory.AccountForSomeAttribute(UserId);
            string sentInvitation = "http://localhost:3001/api/v1/user/friends/invite?" + "uid=" + UserId + "&fid=" + userReceive;
            string acceptInvitation = "http://localhost:3001/api/v1/user/friends/accept?" + "uid=" + userReceive + "&fid=" + UserId;
            string denyInvitation = "http://localhost:3001/api/v1/user/friends/denied?" + "uid=" + userReceive + "&fid=" + UserId;
            string userProfileUrl = "http://localhost:3001/api/v1/user/profile" + "uid=" + UserId;
            string contentFormat = "{{\"UserId\":\"{0}\",\"Message\":{1}}}";
            InviteFriendNotify inviteFriendNotify = new InviteFriendNotify()
            {
                Id = UserId,
                acceptHref = acceptInvitation,
                AvatarUserSentUrl = accountRespone.AvatarUrl,
                denyHref = denyInvitation,
                DisplaynameUserSent = accountRespone.DisplayName,
                inviteHref = sentInvitation,
                onDeleteHref = "",
                onMarkHref = userProfileUrl,
            };
            string messageJson = JsonSerializer.Serialize<object>(inviteFriendNotify);
            string postContent = string.Format(contentFormat, new object[2] { userReceive, messageJson });
            HttpResponseMessage httpResponse =
                await HttpService.PostAsync(NotifiServiceEndpoint, postContent);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> PushMessage_Whencomment(string ownerIdPost, string commentId, string postId)
        {
            AccountResponsitory accountResponsitory = new AccountResponsitory();
            AccountRespone accountRespone = accountResponsitory.AccountForSomeAttribute(UserId);
            string userProfileUrl = "http://localhost:3001/api/v1/user/profile?uid=" + UserId;
            string deleteComment = "http://localhost:3001/api/v1/posts/comments?cid=" + commentId;
            string commentHref = "http://localhost:3001/api/v1/posts/comment?id=" + commentId;
            string postHref = "http://localhost:3001/api/v1/posts?pid=" + postId;
            string contentFormat = "{{\"UserId\":\"{0}\",\"Message\":{1}}}";
            CommentNotify commentNotify = new CommentNotify()
            {
                Id = UserId,
                AvatarUserCommentUrl = accountRespone.AvatarUrl,
                onDeleteHref = deleteComment,
                DisplaynameUserComment = accountRespone.DisplayName,
                commentHref = commentHref,
                onMarkHref = userProfileUrl,
                postHref = postHref
            };
            string messageJson = JsonSerializer.Serialize<object>(commentNotify);
            string postContent = string.Format(contentFormat, new object[2] { ownerIdPost, messageJson });
            HttpResponseMessage httpResponse =
                await HttpService.PostAsync(NotifiServiceEndpoint, postContent);
            return httpResponse.IsSuccessStatusCode;
        }

        public async Task<bool> PushMessage_WhenLike(string ownerIdPost, string postId)
        {
            AccountResponsitory accountResponsitory = new AccountResponsitory();
            AccountRespone accountRespone = accountResponsitory.AccountForSomeAttribute(UserId);
            string userProfileUrl = "http://localhost:3001/api/v1/user/profile?uid=" + ownerIdPost;
            string postHref = "http://localhost:3001/api/v1/posts?pid=" + postId;
            string unlikeHref = "http://localhost:3001/api/v1/posts/likes?pid=" + postId;
            string contentFormat = "{{\"UserId\":\"{0}\",\"Message\":{1}}}";
            LikeNotify likeNotify = new LikeNotify()
            {
                Id = UserId, // nguoi comment UserId => nguoi nhan duoc tb
                AvatarUserCommentUrl = accountRespone.AvatarUrl,
                DisplaynameUserComment = accountRespone.DisplayName,
                onMarkHref = userProfileUrl,
                postHref = postHref,
                onDeleteHref = unlikeHref,
            };
            string messageJson = JsonSerializer.Serialize<object>(likeNotify);
            string postContent = string.Format(contentFormat, new object[2] { ownerIdPost, messageJson });
            HttpResponseMessage httpResponse =
                await HttpService.PostAsync(NotifiServiceEndpoint, postContent);
            return httpResponse.IsSuccessStatusCode;
        }

    }
}