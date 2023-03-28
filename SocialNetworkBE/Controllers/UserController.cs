using Amazon.Runtime.Internal;
using LiteDB;
using MongoDB.Bson;
using ServiceStack.Web;
using SocialNetworkBE.EventHandlers.User;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ObjectId = MongoDB.Bson.ObjectId;

namespace SocialNetworkBE.Controllers {
    public class UserController : ApiController {
        private readonly AccountResponsitory accountResponsitory = new AccountResponsitory();
        private readonly UserEventHandler userEventHandler = new UserEventHandler();
        private const string REFIX = "api/v1/user";

        [HttpGet]
        [Route(REFIX + "/")]
        public ResponseBase UserIdByToken()
        {
            UserMetadata userMetadata =
                 new UserMetadata().GetUserMetadataFromRequest(Request);
            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Get success",
                Data = userMetadata.Id
            };
        }
        [HttpGet]
        [Route(REFIX + "/")] // Endpoint: /api/v1/user?id=507f1f77bcf86cd799439011 [GET]
        public ResponseBase GetUserProfileById(string uid) {
            bool isRightId = ObjectId.TryParse(uid, out var userId);
            if(!isRightId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Wrong format"
                };
            }
            return userEventHandler.GetUserProfileById(userId);
        }

        [HttpGet]
        [Route(REFIX + "/profile")] 
        public ResponseBase GetUserProfileUrlById(string uid)
        {
            bool isRightId = ObjectId.TryParse(uid, out var userId);
            if (!isRightId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Wrong format"
                };
            }
            return userEventHandler.GetUserProfileUrlById(userId);
        }

        [HttpGet]
        [Route(REFIX + "/friends")] // Endpoint: api/v1/user/friends?id=507f1f77bcf86cd799439011&page=1&size=15 [GET] RETURN LIST(ID, DISPLAYNAME, AVATAR, PROFILEURL)
        public ResponseBase GetFriendOfUserByUserId(string uid) {
            bool isRightId = ObjectId.TryParse(uid, out var userId);
            if (!isRightId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Wrong format"
                };
            }
            return userEventHandler.GetFriendOfUserByUserId(userId);
        }

        [HttpGet]
        [Route(REFIX + "/posts")] // Endpoint: /api/v1/user/posts?uid=507f1f77bcf86cd799439011&page=1&size=7 [GET]
        public ResponseBase GetPostByUserId(string uid, Int32 page, Int32 size) { 
            return new ResponseBase(); 
        }

        [HttpPut] // httpput
        [Route(REFIX + "/profile")] // Endpoint: /api/v1/user/profile?uid=507f1f77bcf86cd799439011
        public async Task<ResponseBase> UpdateUserProfileAsync(string uid) {
            var pwd = FormData.GetValueByKey("Password");
            if (pwd == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Password required"
                };
            }

            var email = FormData.GetValueByKey("Email");
            if (email == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Email required"
                };
            }

            var userName = FormData.GetValueByKey("Username");
            if (userName == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Username required"
                };
            }


            var DisplayName = FormData.GetValueByKey("Displayname");
            if (DisplayName == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "DisplayName required"
                };
            }
            bool isRightUserObjectId = ObjectId.TryParse(uid, out var userId);
            if (!isRightUserObjectId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "wrong format"
                };
            }
            HttpFileCollection media = HttpContext.Current.Request.Files;
            var AvatarUrl = media[0];

            AccountRequest accountRequest = new AccountRequest();
            accountRequest.DisplayName = DisplayName;
            accountRequest.Email = email;
            accountRequest.Username = userName;
            accountRequest.Password = pwd;
            accountRequest.Id = userId.ToString();
            return await userEventHandler.UpdateAccount(accountRequest, userId, AvatarUrl); 
        }

        [HttpPost]
        [Route(REFIX + "/friends/invite")] // Endpoint: /api/v1/user/friends/invite?uid={uid}&fid={fid} [POST]
        public ResponseBase SendInvitationToOtherUser(string uid, string fid) {
            bool isRightUserObjectId = ObjectId.TryParse(uid, out var userId);
            bool isRightfriendObjectId = ObjectId.TryParse(fid, out var friendId);
            if (!isRightUserObjectId && !isRightfriendObjectId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "wrong format"
                };
            }
            return userEventHandler.SendInvitationToOtherUser(userId, friendId);
        }

        [HttpPost]
        [Route(REFIX + "/friends/accept")]  // Endpoint: /api/v1/user/friends/accept?uid={uid}&fid={fid} [POST]
        public ResponseBase AcceptInvitationFromOtherUser(string uid, string fid) {
            bool isRightUserObjectId = ObjectId.TryParse(uid, out var userId);
            bool isRightfriendObjectId = ObjectId.TryParse(fid, out var friendId);
            if(!isRightUserObjectId && !isRightfriendObjectId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "wrong format"
                };
            }
            return userEventHandler.AcceptInvitationFromOtherUser(userId, friendId);
        }

        [HttpPost]
        [Route(REFIX + "/friends/denied")] // Endpoint: /api/v1/user/friends/denied?uid={uid}&fid={fid} [POST]
        public ResponseBase DeniedInvatationToOtherUser(string uid, string fid) {
            bool isRightUserObjectId = ObjectId.TryParse(uid, out var userId);
            bool isRightfriendObjectId = ObjectId.TryParse(fid, out var friendId);
            if (!isRightUserObjectId && !isRightfriendObjectId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "wrong format"
                };
            }
            return userEventHandler.DeniedInvatationToOtherUser(userId, friendId);
        }

        [HttpPost]
        [Route(REFIX + "/friends/remove")] // Endpoint: /api/v1/user/friends/denied?uid={uid}&fid={fid} [POST]
        public ResponseBase RemoveFriend(string uid, string fid)
        {
            bool isRightUserObjectId = ObjectId.TryParse(uid, out var userId);
            bool isRightfriendObjectId = ObjectId.TryParse(fid, out var friendId);
            if (!isRightUserObjectId && !isRightfriendObjectId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "wrong format"
                };
            }
            return userEventHandler.RemoveAFriendFromAccount(userId, friendId);
        }
    }
}