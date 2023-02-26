using SocialNetworkBE.Payload.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    public class PostController {
        private const string REFIX = "api/v1/posts";

        [HttpGet]
        [Route(REFIX + "/")] // Endpoint: /api/v1/posts?page=1&size=10 [GET]: 
        public ResponseBase GetPostListWithPaging(int page, int size) {
            return new ResponseBase();
        }
        [HttpPost]
        [Route(REFIX + "/")] // Endpoint: /api/v1/posts/ [POST]
        public ResponseBase CreateAPostFromUser() {
            return new ResponseBase();
        }

        [HttpGet]
        [Route(REFIX + "/comments")] // Endpoint: /api/v1/post/comments/?pid={postid}&page=1&size=5&sort=desc [GET]:
        public ResponseBase GetCommentOfPostById(string pid, int page, int size, string sort) {
            return new ResponseBase();
        }

        [HttpPost]
        [Route(REFIX + "/comments")] // Endpoint: /api/v1/post/comments/?pid={postid} [POST]:
        public ResponseBase CommentAPostById(string pid) {
            return new ResponseBase();
        }

        [HttpDelete]
        [Route(REFIX + "/comments")] // Endpoint: /api/v1/post/comments?pid={postid}&cid={commentid}  [DELETE]:
        public ResponseBase DeleteCommentOfPostByPostIdAndCommentId(string pid, string cid) {
            return new ResponseBase();
        }

        [HttpGet]
        [Route(REFIX + "/likes")] // Endpoint:/api/v1/post/likes?pid={postid}page=1&size=10&sort=desc [GET]:
        public ResponseBase GetLikesOfPostById(string pid, int page, int size, string sort) {
            return new ResponseBase();
        }

        [HttpPost]
        [Route(REFIX + "/likes")] // Endpoint:  /api/v1/post/likes?pid={postid}&uid={uid} [POST]:
        public ResponseBase LikeAPostByPostId(string pid, string uid) {
            return new ResponseBase();
        }

        [HttpDelete]
        [Route(REFIX + "/likes")] // Endpoint:  /api/v1/post/likes?pid={postid}&lid={likeid} [DELETE]:
        public ResponseBase UnLikePostByPostId(string pid, string lid) {
            return new ResponseBase();
        }
    }
}