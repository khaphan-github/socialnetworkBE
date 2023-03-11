using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Services.Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.EventHandlers.PostHandler {
    public class PostEventHandler {

        private readonly PostRespository postRespository = new PostRespository();
        public ResponseBase GetPostsWithPaging(int page, int size) {

            List<PostResponse> postResponses = postRespository
             .GetPostByPageAndSizeAndSorted(page, size)
             .Select(bsonPost => BsonSerializer.Deserialize<PostResponse>(bsonPost))
             .ToList();


            if (postResponses.Count == 0) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Empty post",
                };
            }

            // Logic: page index endpoint
            string pagingEndpoint = "/api/v1/posts/current?page=";
            string pagingSize = "&size=" + size.ToString();

            string nextPageURL = pagingEndpoint + (page + 1).ToString() + pagingSize;
            string previousPageURL = pagingEndpoint;

            if (page == 0)
                previousPageURL += page.ToString() + pagingSize;
            else
                previousPageURL += (page - 1).ToString() + pagingSize;


            PagingResponse pagingResponse = new PagingResponse() {
                Paging = postResponses,
                NextPageURL = nextPageURL,
                PreviousPageURL = previousPageURL,
            };

            ResponseBase response = new ResponseBase() {
                Status = Status.Success,
                Message = "Get post success",
                Data = pagingResponse
            };

            return response;
        }

        public ResponseBase HandleUserCreateNewPost(
            HttpFileCollection Media,
            string Content,
            string OwnerId,
            string OwnerAvatarURL,
            string OwnerDisplayName,
            string OwnerProfileURL) {

            FirebaseImage firebaseService = new FirebaseImage();
            List<string> mediaURLList = new List<string>();

            if (Media != null) {
                foreach(var media in Media) {
                    
                }
                // TODO: Handle Upload image;

            }

            Post newPost = new Post() {
                Id = ObjectId.GenerateNewId(),
                CreateDate = DateTime.Now,
                UpdateAt = DateTime.Now,
                OwnerAvatarURL = OwnerAvatarURL,
                OwnerId = ObjectId.Parse(OwnerId),
                OwnerDisplayName = OwnerDisplayName,
                OwnerProfileURL = OwnerProfileURL,
                Content = Content,
                Media = mediaURLList,
            };

            newPost.CommentsURL = "/api/v1/post/comments?pid=" + newPost.Id.ToString();
            newPost.LikesURL = "/api/v1/post/likes?pid=" + newPost.Id.ToString();

            Post savedPost = postRespository.CreateNewPost(newPost);
            if (savedPost == null) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Create post failure"
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Create post success",
                Data = PostResponse.ConvertPostToPostResponse(savedPost)
            };
        }
        public ResponseBase DeletePostById(ObjectId id) {

            bool isDeleted = postRespository.DetetePostById(id);

            if (!isDeleted) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Delete failure"
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Detete success"
            };
        }
        public ResponseBase GetCommentOfPostByPostId(ObjectId postObjectId, int page, int size) {
            List<Comment> bsonDocumentComment = 
                postRespository.GetCommentsByPostIdWithPaging(postObjectId, page, size);
            
            if (bsonDocumentComment.Count == 0) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "This post have no comment"
                };
            }
            
            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get comment success",
                Data = bsonDocumentComment
            };
        }

        public ResponseBase HandleCommentAPostById() {
            return new ResponseBase() { };
        }

    }
}