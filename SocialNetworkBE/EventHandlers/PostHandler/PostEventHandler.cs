using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.DataTranfers;
using SocialNetworkBE.Services.Firebase;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

namespace SocialNetworkBE.EventHandlers.PostHandler {
    public class PostEventHandler
    {

        private readonly PostRespository PostRespository = new PostRespository();
        private readonly CommentRepository CommentRepository = new CommentRepository();
        private readonly AccountResponsitory AccountRepostitory = new AccountResponsitory();
        private IMongoCollection<Post> PostCollection { get; set; }
        private IMongoDatabase DatabaseConnected { get; set; }
        private const string PostDocumentName = "Post";

        public PostEventHandler()
        {
            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            DatabaseConnected = MongoDatabase.GetMongoDBConnected();

            PostCollection = DatabaseConnected.GetCollection<Post>(PostDocumentName);
        }
        public async Task<ResponseBase> GetPostsWithPaging(UserMetadata userMetadata, int page, int size)
        {
            // TODO: Get like post
            List<PostDataTranfer> postResponses =
                await PostRespository.GetSortedAndProjectedPostsAsync(ObjectId.Parse(userMetadata.Id), page, size);

            if (postResponses == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Empty post",
                };
            }

            // Logic: page index endpoint
            string pagingEndpoint = "/api/v1/posts/current?";
            PagingResponse pagingResponse =
                new PagingResponse(pagingEndpoint, page, size, postResponses); ResponseBase response = new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Get post success",
                    Data = pagingResponse
                };

            return response;
        }

        public async Task<ResponseBase> GetPostsOfFriendWithPaging(UserMetadata userMetadata, int page, int size)
        {
            // TODO: Get like post
            bool isRightCommentId = ObjectId.TryParse(userMetadata.Id, out var userId);
            List<ObjectId> listFriend = AccountRepostitory.ListFriendByUserId(userId);
            List<PostDataTranfer> AllFriendPost = new List<PostDataTranfer>();
            foreach(ObjectId i in listFriend)
            {
                AllFriendPost.AddRange(await PostRespository.GetSortedAndProjectedPostsOfFriendAsync(userId, i, page, size));
            }
            if (AllFriendPost == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Empty post",
                };
            }

            // Logic: page index endpoint
            string pagingEndpoint = "/api/v1/posts/current?";
            PagingResponse pagingResponse =
                new PagingResponse(pagingEndpoint, page, size, AllFriendPost); ResponseBase response = new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Get post success",
                    Data = pagingResponse
                };

            return response;
        }

        public async Task<ResponseBase> GetPostsOfuserWithPaging(UserMetadata userMetadata, int page, int size)
        {
            // TODO: Get like post
            List<PostDataTranfer> postResponses =
                await PostRespository.GetSortedAndProjectedPostsOfUserAsync(ObjectId.Parse(userMetadata.Id), page, size);

            if (postResponses == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Empty post",
                };
            }

            // Logic: page index endpoint
            string pagingEndpoint = "/api/v1/posts/current?";
            PagingResponse pagingResponse =
                new PagingResponse(pagingEndpoint, page, size, postResponses); ResponseBase response = new ResponseBase()
                {
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

                    await firebaseService.UploadImageAsync(Media[i].InputStream, folder, mediaName).ConfigureAwait(false);

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
            newPost.Likes = new List<ObjectId>();

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

        public async Task<ResponseBase> GetCommentOfPostByPostId(ObjectId postObjectId,ObjectId? parentCommentId, int page, int size) {
            List<CommentDataTranfer> commentOfPostWithPaging;
            if (parentCommentId == null) {
                commentOfPostWithPaging = await CommentRepository.GetCommentOfPostWithPaging(postObjectId, null, page, size);
            }
            else {
                commentOfPostWithPaging = await CommentRepository.GetCommentOfPostWithPaging(postObjectId, parentCommentId, page, size);
            }


            if (commentOfPostWithPaging == null) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Get comment failure - comment of post is empty",
                };
            }
            const string commentEndpoint = "/api/v1/posts/comments/";

            List<CommentsResponse> commentResponses = 
                commentOfPostWithPaging
                .Select(comment => new CommentsResponse() {
                    ActionCount= comment.ActionCount,
                    ActionUser= comment.ActionUser,
                    ChildCount = comment.CommentCount,
                    ChildrenHref = comment.CommentCount > 0 ? commentEndpoint + "?parent=" + comment.Id : "",
                    Content = comment.Content,
                    CreateDate= comment.CreateDate,
                    Id= comment.Id,
                    IsHaveChild = comment.CommentCount > 0,
                    OwnerAvatarURL= comment.OwnerAvatarURL,
                    OwnerDisplayName= comment.OwnerDisplayName,
                    OwnerId= comment.OwnerId,
                    OwnerProfileURL= comment.OwnerProfileURL,
                    ParentId= comment.ParentId,
                    PostId = comment.PostId,
                })
                .ToList();

            string pagingEndpoint = "/api/v1/posts/current?pid=" + postObjectId.ToString() + "&";
            PagingResponse pagingResponse =
                new PagingResponse(pagingEndpoint, page, commentResponses.Count, commentResponses);

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get comment success",
                Data = pagingResponse
            };
        }


        public async Task<ResponseBase> CommentAPostByPostId(
            ObjectId postId,
            ObjectId? commentId,
            UserMetadata userMetadata,
            string comment) {

            Comment commentToCreate = new Comment() {
                Id = ObjectId.GenerateNewId(),
                Content = comment,
                PostId = postId,
                OwnerId = ObjectId.Parse(userMetadata.Id),
            };

            if (commentId != null) {
                commentToCreate.ParentId = commentId;
                ObjectId parentObjectId = ObjectId.Parse(commentId.ToString());

                await CommentRepository
                    .UpdateNumberOfChildrent(parentObjectId, 1)
                    .ConfigureAwait(false);
            }

            Comment commentCreated = CommentRepository.CreateCommentAPost(commentToCreate);

            await PostRespository.UpdateNumOfCommentOfPost(postId, 1).ConfigureAwait(false);

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

        public async Task<ResponseBase> DeleteCommentByCommentId(ObjectId commentId, ObjectId postId, UserMetadata userMetadata) {

            DeleteResult deleteResult =
                CommentRepository.DeteteCommentById(commentId, ObjectId.Parse(userMetadata.Id));

            if (deleteResult.IsAcknowledged) {
                await PostRespository.UpdateNumOfCommentOfPost(postId, -1);

                return new ResponseBase() {
                    Status = Status.Success,
                    Message = "Delete comment success",
                };
            }
            return new ResponseBase() {
                Status = Status.Failure,
                Message = "Delete comment failure",
            };

        }

        public async Task<ResponseBase> UpdateAPost(ObjectId postId, HttpFileCollection Media,
            string Content)
        {

            FirebaseImage firebaseService = new FirebaseImage();

            int MaxMediaInCollection = 10;
            FilterDefinition<Post> postFilter =
                    Builders<Post>.Filter.Eq("_id", postId);

            Post post = PostCollection.Find(postFilter).FirstOrDefault();
            if (post == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Can't find Post",
                };
            }
            int sizeMediaPost = post.Media.Count();

            if (Media != null && Media.Count > MaxMediaInCollection)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Number of file must smaller than 10 file",
                };
            }
            
            List<string> medianame = new List<string>();
            for(int i = 0; i < sizeMediaPost; i++)
            {
                var mediaTemp = post.Media[i].Split('/').Last();
                Debug.WriteLine(mediaTemp);
                medianame.Add(mediaTemp);
            }    

            if (Media != null)
            {
                for (int i = 0; i < Media.Count; i++)
                {
                    string folder = "PostMedia";
                    if (i < sizeMediaPost)
                    {
                        await firebaseService.UploadImageAsync(Media[i].InputStream, folder, medianame[i]).ConfigureAwait(false);
                    }
                    else
                    {
                        string mediaNameNew = Guid.NewGuid().ToString() + ".png";
                        await firebaseService.UploadImageAsync(Media[i].InputStream, folder, mediaNameNew).ConfigureAwait(false);

                        string imageDownloadLink2 = firebaseService.StorageDomain + "/" + folder + "/" + mediaNameNew;
                        post.Media.Add(imageDownloadLink2);
                    }
                }
            }
            post.Content = Content;
            Post updateResult =
            await PostRespository.UpdateAPost(postId, post);

            if (updateResult == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Update failure",
                };
            }

            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Update success",
                Data = updateResult
            };

        }

        public async Task<ResponseBase> GetCommentById(ObjectId commentId) {
            // TODO: Get commetn by id
            CommentDataTranfer commentDataTranfer = await CommentRepository.GetCommentById(commentId);
            if(commentDataTranfer == null) {
                return new ResponseBase() {
                    Status = Status.Success,
                    Message = "Comment not exist",
                };
            }
            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get comment success",
                Data = commentDataTranfer,
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
                Message = "Update comment success",
            };
        }
        public async Task<ResponseBase> GetLikesOfPostById(ObjectId postId, int page, int size) {

            BsonDocument likesOfPost =
                PostRespository.GetUserMetadataLikedPost(postId, page, size);

            List<ObjectId> userLikedId =
                likesOfPost["Likes"].AsBsonArray
                .Select(objectId => ObjectId.Parse(objectId.ToString()))
                .ToList();


            if (userLikedId.Count == 0) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "There are not like in this post",
                };
            }

            List<BsonDocument> likesBsonDocument =
                await AccountRepostitory.GetListAccountsMetadata(userLikedId, page, size).ConfigureAwait(false);

            List<LikeResponse> likesResponse =
                likesBsonDocument.Select(bson => BsonSerializer.Deserialize<LikeResponse>(bson)).ToList();


            string pagingEndpoint = "/api/v1/posts/likes?";
            PagingResponse pagingResponse = 
                new PagingResponse(pagingEndpoint,page, likesResponse.Count, likesResponse) ;

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get list user like post success",
                Data = pagingResponse
            };
        }

        public async Task<ResponseBase> LikeAPostByPostId(ObjectId postId, UserMetadata userMetadata) {
            try {
                await PostRespository.MakeALikeOfPostAsync(postId, ObjectId.Parse(userMetadata.Id));

                return new ResponseBase() {
                    Status = Status.Success,
                    Message = "Like Success",
                };

            } catch (Exception) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Like Failure",
                };
            }
        }

        public async Task<ResponseBase> UnLikeAPostByPostId(ObjectId postId, UserMetadata userMetadata) {
            try {
                await PostRespository.RemoveAlikeOfPostAsync(postId, ObjectId.Parse(userMetadata.Id));

                return new ResponseBase() {
                    Status = Status.Success,
                    Message = "Unlike Success",
                };

            } catch (Exception) {
                return new ResponseBase() {
                    Status = Status.Failure,
                    Message = "Unlike Failure",
                };
            }
        }
    }
}
