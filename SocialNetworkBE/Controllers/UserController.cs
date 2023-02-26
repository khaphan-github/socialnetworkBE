using SocialNetworkBE.Payload.Response;
using System;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    public class UserController : ApiController {

        private const string REFIX = "api/v1/user";
        [HttpGet]
        [Route(REFIX + "/")] // Endpoint: /api/v1/user?id=507f1f77bcf86cd799439011 [GET]
        public ResponseBase GetUserProfileById(string uid) {
            return new ResponseBase();
        }

        [HttpGet]
        [Route(REFIX + "/friends")] // Endpoint: api/v1/user/friends?id=507f1f77bcf86cd799439011&page=1&size=15 [GET]
        public ResponseBase GetFriendOfUserByUserId(string uid, Int32 page, Int32 size) { 
            return new ResponseBase(); 
        }

        [HttpGet]
        [Route(REFIX + "/posts")] // Endpoint: /api/v1/user/posts?uid=507f1f77bcf86cd799439011&page=1&size=7 [GET]
        public ResponseBase GetPostByUserId(string uid, Int32 page, Int32 size) { 
            return new ResponseBase(); 
        }

        [HttpPost]
        [Route(REFIX + "/profile")] // Endpoint: /api/v1/user/profile?uid=507f1f77bcf86cd799439011
        public ResponseBase UpdateUserProfile(string uid) { 
            return new ResponseBase(); 
        }

        [HttpPost]
        [Route(REFIX + "/friends/invite")] // Endpoint: /api/v1/user/friends/invite?uid={uid}&fid={fid} [POST]
        public ResponseBase SendInvitationToOtherUser(string uid, string fid) {
            return new ResponseBase();
        }

        [HttpPost]
        [Route(REFIX + "/friends/accept")]  // Endpoint: /api/v1/user/friends/accept?uid={uid}&fid={fid} [POST]
        public ResponseBase AcceptInvitationFromOtherUser(string uid, string fid) {
            return new ResponseBase();
        }

        [HttpPost]
        [Route(REFIX + "/friends/denied")] // Endpoint: /api/v1/user/friends/denied?uid={uid}&fid={fid} [POST]
        public ResponseBase DeniedInvatationToOtherUser(string uid, string fid) {
            return new ResponseBase();
        }
    }
}