﻿using MongoDB.Bson;
using SocialNetworkBE.EventHandlers.PostHandler;
using SocialNetworkBE.Payload.Response;
using System;
using System.Web;
using System.Web.Http;
using SocialNetworkBE.Payloads.Request;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("api/v1/posts")]
    public class PostController : ApiController {

        private readonly PostEventHandler postEventHandler = new PostEventHandler();

        [HttpPost]
        [Route("")] 
        // Endpoint: /api/v1/posts/ [POST]
        public ResponseBase CreateAPostFromUser() {
            HttpFileCollection Media = HttpContext.Current.Request.Files;

            var Content = FormData.GetValueByKey("Content");
            if (Content == null) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Content required"
                };
            }

            var OwnerId = FormData.GetValueByKey("OwnerId");
            if (OwnerId == null) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "OwnerId required"
                };
            }

            var OwnerAvatarURL = FormData.GetValueByKey("OwnerAvatarURL");
            if (OwnerAvatarURL == null) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "OwnerAvatarURL required"
                };
            }

            var OwnerDisplayName = FormData.GetValueByKey("OwnerDisplayName");
            if (OwnerDisplayName == null) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "OwnerDisplayName required"
                };
            }

            var OwnerProfileURL = FormData.GetValueByKey("OwnerProfileURL");
            if (OwnerProfileURL == null) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "OwnerProfileURL required"
                };
            }

            return postEventHandler
                .HandleUserCreateNewPost(Media, Content, OwnerId, OwnerAvatarURL, OwnerDisplayName, OwnerProfileURL);
        }
        
        [HttpDelete]
        [Route("")]
        public ResponseBase DetetePostById(string pid) {
            bool isRightObjectId = ObjectId.TryParse(pid, out var id);
            if(!isRightObjectId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "ObjectId Wrong Format"
                };
            }

            return postEventHandler.DeletePostById(id);
        }
        
        [HttpPost]
        [Route("current")] 
        // Endpoint: /api/v1/posts?page=1&size=10 [POST]: 
        public ResponseBase GetPostListWithPaging(int page, int size) {

            if (page <= 0) page = 0;
            if (size <= 0) size = 1;
            if (size > 20) size = 20;

            return postEventHandler.GetPostsWithPaging(page, size);
        }

        [HttpGet]
        [Route("comments")] 
        // Endpoint: /api/v1/post/comments/?pid={postid}&page=1&size=1 [POST]:
        public ResponseBase GetCommentsOfPostById(string pid, int page, int size) {

            if (string.IsNullOrWhiteSpace(pid)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "This request require pid, page , size"
                };
            }

            bool isRightObjectId = ObjectId.TryParse(pid, out var id);
            if (!isRightObjectId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "ObjectId wrong format"
                };
            }

            if (page < 0) page = 0;
            if (size < 0) size = 1;
            if (size > 20) size = 20;

            return postEventHandler.GetCommentOfPostByPostId(id, page, size);
        }
        [HttpGet]
        [Route("comments")]
        // Endpoint: /api/v1/post/comments/?pid={postid}&page=1&size=1 [POST]:
        public ResponseBase GetCommentsOfPostByIdAndCommentId(string pid, string cid, int page, int size) {

            if (string.IsNullOrWhiteSpace(pid) || string.IsNullOrWhiteSpace(cid)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "This request require pid, cid, page, size"
                };
            }

            bool isRightPostId = ObjectId.TryParse(pid, out var postId);
            if (!isRightPostId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "pid wrong format object id"
                };
            }

            bool isRightCommentParentId = ObjectId.TryParse(cid, out var commentId);
            if (!isRightCommentParentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "cid wrong format object id"
                };
            }

            if (page < 0) page = 0;
            if (size <= 0) size = 1;
            if (size > 20) size = 20;

            return postEventHandler.GetCommentOfPostByParentId(postId,commentId, page, size);
        }

        [HttpPost]
        [Route("comments")] 
        // Endpoint: /api/v1/post/comments/?pid={postid} [POST]:
        public ResponseBase CommentAPostById([FromBody] CommentRequest commentRequest) {
            return new ResponseBase();
        }

        [HttpDelete]
        [Route("comments")] 
        // Endpoint: /api/v1/post/comments?pid={postid}&cid={commentid}  [DELETE]:
        public ResponseBase DeleteCommentOfPostByPostIdAndCommentId(string pid, string cid) {
            return new ResponseBase();
        }

        [HttpGet]
        [Route("likes")] 
        // Endpoint:/api/v1/post/likes?pid={postid}page=1&size=10&sort=desc [GET]:
        public ResponseBase GetLikesOfPostById(string pid, Int32? page, Int32? size, string sort) {
            return new ResponseBase();
        }

        [HttpPost]
        [Route("likes")] // Endpoint:  /api/v1/post/likes?pid={postid}&uid={uid} [POST]:
        public ResponseBase LikeAPostByPostId(string pid, string uid) {
            return new ResponseBase();
        }

        [HttpDelete]
        [Route("likes")] // Endpoint:  /api/v1/post/likes?pid={postid}&lid={likeid} [DELETE]:
        public ResponseBase UnLikePostByPostId(string pid, string lid) {
            return new ResponseBase();
        }
    }
}