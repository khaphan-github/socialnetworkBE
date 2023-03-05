﻿using MongoDB.Bson;
using Org.BouncyCastle.Asn1.X509.Qualified;
using SocialNetworkBE.EventHandlers.PostHandler;
using SocialNetworkBE.Payload.Response;
using System;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("api/v1/posts")]
    public class PostController : ApiController {

        private readonly PostEventHandler postEventHandler = new PostEventHandler();

        [HttpPost]
        [Route("")] // Endpoint: /api/v1/posts/ [POST]
        public ResponseBase CreateAPostFromUser() {
            return new ResponseBase();
        }

        [HttpPost]
        [Route("current")] // Endpoint: /api/v1/posts?page=1&size=10 [GET]: 
        public ResponseBase GetPostListWithPaging(int page, int size) {
            if (page < 0) {
                page = 0;
            }
            if (size < 0) {
                size = 1;
            }
             if(size > 20) {
                size = 20;
            }
            return postEventHandler.GetPostsWithPaging(page, size);
        }
        
        [HttpGet]
        [Route("comments")] // Endpoint: /api/v1/post/comments/?pid={postid}&page=1&size=1 [GET]:
        public ResponseBase GetCommentOfPostById(string pid, int page, int size) {

            if(pid == "") {
                return new ResponseBase() {
                       Status = Status.WrongFormat,
                       Message = "This request require pid but you missing"
                };
            }

            ObjectId postId;

            if(!ObjectId.TryParse(pid, out postId)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Value of param pid is wrong format objectId"
                };
            }

            if (page < 0) {
                page = 0;
            }
            if (size < 0) {
                size = 1;
            }
            if (size > 20) {
                size = 20;
            }

            return postEventHandler.GetCommentOfPostByPostId(postId, page, size);
        }

        [HttpPost]
        [Route("comments")] // Endpoint: /api/v1/post/comments/?pid={postid} [POST]:
        public ResponseBase CommentAPostById(string pid) {
            return new ResponseBase();
        }

        [HttpDelete]
        [Route("comments")] // Endpoint: /api/v1/post/comments?pid={postid}&cid={commentid}  [DELETE]:
        public ResponseBase DeleteCommentOfPostByPostIdAndCommentId(string pid, string cid) {
            return new ResponseBase();
        }

        [HttpGet]
        [Route("likes")] // Endpoint:/api/v1/post/likes?pid={postid}page=1&size=10&sort=desc [GET]:
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