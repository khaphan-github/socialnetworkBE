using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Services.Firebase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetworkBE.EventHandlers.PostHandler {
    public class PostEventHandler {

        private readonly PostRespository PostRespository = new PostRespository();
        private readonly CommentRepository CommentRepository = new CommentRepository();
        public ResponseBase GetPostsWithPaging(int page, int size) {

            List<PostResponse> postResponses = PostRespository
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

        public async Task<ResponseBase> HandleUserCreateNewPost(
            HttpFileCollection Media,
            string Content,
           UserMetadata userMetadata) {

            FirebaseImage firebaseService = new FirebaseImage();
            List<string> mediaURLList = new List<string>();

            int MaxMediaInCollection = 10;
            if (Media != null && Media.Count > MaxMediaInCollection) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Number of file must smaller than 10 file",
                };
            }

            if (Media != null) {
                for (int i = 0; i < Media.Count; i++) {
                    string mediaName = Guid.NewGuid().ToString() + ".png";

                    string folder = "PostMedia";

                    await firebaseService.UploadImageAsync(Media[i].InputStream, folder, mediaName);

                    string imageDownloadLink = firebaseService.StorageDomain + "/" + folder + "/" + mediaName;

                    mediaURLList.Add(imageDownloadLink);
                }
            }

            Post newPost = new Post() {
                Id = ObjectId.GenerateNewId(),
                CreateDate = DateTime.Now,
                UpdateAt = DateTime.Now,
                OwnerAvatarURL = userMetadata.AvatarURL,
                OwnerId = ObjectId.Parse(userMetadata.Id),
                OwnerDisplayName = userMetadata.DisplayName,
                OwnerProfileURL = userMetadata.UserProfileUrl,
                Content = Content,
                Media = mediaURLList,
            };

            newPost.CommentsURL = "/api/v1/post/comments?pid=" + newPost.Id.ToString();
            newPost.LikesURL = "/api/v1/post/likes?pid=" + newPost.Id.ToString();

            Post savedPost = PostRespository.CreateNewPost(newPost);
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
        public ResponseBase DeletePostById(ObjectId id, ObjectId ownerId) {

            bool isDeleted = PostRespository.DetetePostById(id, ownerId);

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

            List<Comment> commentOfPostWithPaging =
                CommentRepository.GetCommentOfPostWithPaging(postObjectId, page, size);

            if (commentOfPostWithPaging.Count == 0) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Get comment failure - comment of post is empty",
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get comment success",
                Data = commentOfPostWithPaging
            };
        }

        public ResponseBase GetCommentOfPostByParentId(ObjectId postId, ObjectId commentId, int page, int size) {

            List<Comment> commentOfPostWithPaging =
                CommentRepository.GetChildrenCommentsByParentId(postId, commentId, page, size);

            if (commentOfPostWithPaging.Count == 0) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Get comment failure - comment of post is empty",
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get comment success",
                Data = commentOfPostWithPaging
            };
        }

        public ResponseBase CommentAPostByPostId(
            ObjectId postId,
            ObjectId? commentId,
            UserMetadata userMetadata,
            string comment) {

            Comment commentToCreate = new Comment() {
                Id = ObjectId.GenerateNewId(),
                Content = comment,
                PostId = postId,
                OwnerId = ObjectId.Parse(userMetadata.Id),
                OwnerAvatarURL = userMetadata.AvatarURL,
                OwnerDisplayName = userMetadata.DisplayName,
                OwnerProfileURL = userMetadata.UserProfileUrl,
            };

            if (commentId != null) {
                commentToCreate.ParentId = commentId;
            }

            Comment commentCreated = CommentRepository.CreateCommentAPost(commentToCreate);

            if (commentCreated == null) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Comment failure",
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Comment success",
                Data = commentCreated
            };
        }

        public ResponseBase DeleteCommentByCommentId(ObjectId commentId, UserMetadata userMetadata) {

            DeleteResult deleteResult =
                CommentRepository.DeteteCommentById(commentId, ObjectId.Parse(userMetadata.Id));

            if (!deleteResult.IsAcknowledged) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Delete comment failure",
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Delete comment success",
            };
        }

        public ResponseBase UpdateCommentById(ObjectId commentId, UserMetadata userMetadata, string content) {

            UpdateResult updateResult =
                CommentRepository.UpdateCommentByComentId(commentId, ObjectId.Parse(userMetadata.Id), content);

            if (!updateResult.IsAcknowledged) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Update comment failure",
                };
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Update commentt success",
            };
        }

        public ResponseBase GetLikesOfPostById(ObjectId postId, int page, int size, string sort) {
            return new ResponseBase() {
                Status = Status.Success,
                Message = "Update commentt success",
            };
        }
    }
}