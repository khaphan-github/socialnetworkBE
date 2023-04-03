using MongoDB.Bson;
using SocialNetworkBE.EventHandlers.PostHandler;
using SocialNetworkBE.Payload.Response;
using System.Web;
using System.Web.Http;
using SocialNetworkBE.Payloads.Request;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using ServiceStack.Messaging;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.EventHandlers.User;
using Firebase.Auth;
using static ServiceStack.Diagnostics.Events;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Services.Notification;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("api/v1/posts")]
    public class PostController : ApiController {

        private readonly PostEventHandler postEventHandler = new PostEventHandler();
        private PushNotificationService _pushNotificationService;
        private PostRespository PostRespository;
        [HttpGet]
        [Route("")] 
        public ResponseBase GetPostById(string pid)
        {
            bool isRightId = ObjectId.TryParse(pid, out var postId);
            if (!isRightId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Wrong format"
                };
            }
            return postEventHandler.GetPostById(postId);
        }


        [HttpPost]
        [Route("")]
        // Endpoint: /api/v1/posts/ [POST]
        public async Task<ResponseBase> CreateAPostFromUser() {
            HttpFileCollection Media = HttpContext.Current.Request.Files;
            var Content = FormData.GetValueByKey("Content");

            if (Content == null) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Content required"
                };
            }
            UserMetadata userMetadata =
                 new UserMetadata().GetUserMetadataFromRequest(Request);

            return await postEventHandler
                .HandleUserCreateNewPost(Media, Content, userMetadata);
        }

        

        [HttpDelete]
        [Route("")]
        public ResponseBase DetetePostById(string pid) {
            // TODO: Need to test
            bool isRightObjectId = ObjectId.TryParse(pid, out var id);
            if (!isRightObjectId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "ObjectId Wrong Format"
                };
            }
            UserMetadata userMetadata =
                new UserMetadata().GetUserMetadataFromRequest(Request);

            return postEventHandler.DeletePostById(id, ObjectId.Parse(userMetadata.Id));
        }

        //[HttpPost]
        //[Route("current")]
        //// Endpoint: /api/v1/posts?page=1&size=10 [POST]: 
        //public async Task<ResponseBase> GetPostListWithPaging(int page, int size) {
        //    if (page <= 0) page = 0;
        //    if (size <= 0) size = 1;
        //    if (size > 20) size = 20;
        //    UserMetadata userMetadata =
        //       new UserMetadata().GetUserMetadataFromRequest(Request);
        //    return await postEventHandler.GetPostsWithPaging(userMetadata, page, size);
        //}

        [HttpGet]
        [Route("current")]
        // Endpoint: /api/v1/posts?page=1&size=10 [POST]: 
        public async Task<ResponseBase> GetPostFriendListWithPaging(int page, int size)
        {
            if (page <= 0) page = 0;
            if (size <= 0) size = 1;
            if (size > 20) size = 20;
            UserMetadata userMetadata =
               new UserMetadata().GetUserMetadataFromRequest(Request);
            return await postEventHandler.GetPostsOfFriendWithPaging(userMetadata, page, size);
        }

        [HttpGet]
        [Route("user")]
        // Endpoint: /api/v1/posts?page=1&size=10 [POST]: 
        public async Task<ResponseBase> GetPostUserListWithPaging(string userId,int page, int size)
        {
            if (page <= 0) page = 0;
            if (size <= 0) size = 1;
            if (size > 20) size = 20;
            return await postEventHandler.GetPostsOfuserWithPaging(ObjectId.Parse(userId), page, size);
        }

        [Route("comment")]
        // Endpoint: /api/v1/post/comments/?pid={postid}&page=1&size=1 [POST]:
        public async Task<ResponseBase> GetCommentsById(string id) {
            bool isRightPostId = ObjectId.TryParse(id, out var commentId);
            if (!isRightPostId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "pid wrong format object id"
                };
            }
            return await postEventHandler.GetCommentById(commentId);
        }

        [HttpGet]
        [Route("comments")]
        // Endpoint: /api/v1/post/comments/?pid={postid}&page=1&size=1 [POST]:
        public async Task<ResponseBase> GetCommentsOfPostById(string pid, string cid, int page, int size) {
            if (string.IsNullOrWhiteSpace(pid)) {
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
            if (page < 0) page = 0;
            if (size <= 0) size = 1;
            if (size > 20) size = 20;

            bool isRightCommentid = ObjectId.TryParse(cid, out var commentId);
            if(!isRightCommentid) {
                return await postEventHandler.GetCommentOfPostByPostId(postId, null, page, size);
            }
            return await postEventHandler.GetCommentOfPostByPostId(postId, commentId, page, size);
        }



        [HttpPost]
        [Route("comments")]
        // Endpoint: /api/v1/post/c  [POST]:
        public async Task<ResponseBase> CommentAPostById([FromBody] CommentRequest commentRequest) {
            if (string.IsNullOrWhiteSpace(commentRequest.Comment)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Comment is null"
                };
            }

            bool isRightPostId = ObjectId.TryParse(commentRequest.PostId.ToString(), out var postId);
            if (!isRightPostId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Post id wrong format object id"
                };
            }

            UserMetadata userMetadata =
               new UserMetadata().GetUserMetadataFromRequest(Request);

            if (string.IsNullOrWhiteSpace(commentRequest.CommentId)) {
                return await postEventHandler.CommentAPostByPostId(postId, null, userMetadata, commentRequest.Comment);
            }

            bool isRightCommentId = ObjectId.TryParse(commentRequest.CommentId.ToString(), out var commentId);
            if (!isRightCommentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "Comment id wrong format object id"
                };
            }
            return await postEventHandler
                .CommentAPostByPostId(postId, commentId, userMetadata, commentRequest.Comment);
        }

        [HttpDelete]
        [Route("comments")]
        // Endpoint: /api/v1/post/comments?pid={postid}&cid={commentid}  [DELETE]:
        public async Task<ResponseBase> DeleteCommentOfPostByPostIdCommentIdAndCommentId(string pid, string cid) {
            if (string.IsNullOrWhiteSpace(cid)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "cid is null or white space"
                };
            }
            bool isRightCommentId = ObjectId.TryParse(cid, out var commentId);
            if (!isRightCommentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "comment id wrong format object id"
                };
            }
            bool isRightPostId = ObjectId.TryParse(cid, out var postId);
            if (!isRightPostId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "post id wrong format object id"
                };
            }
            UserMetadata userMetadata =
               new UserMetadata().GetUserMetadataFromRequest(Request);

            return await postEventHandler.DeleteCommentByCommentId(commentId, postId, userMetadata);
        }

        [HttpPut]
        [Route("comments")]
        public ResponseBase UpdateACommentById([FromBody] CommentUpdate commentUpdate) {

            if (string.IsNullOrWhiteSpace(commentUpdate.CommentId)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "cid is null or white space"
                };
            }

            bool isRightCommentId = ObjectId.TryParse(commentUpdate.CommentId, out var commentId);
            if (!isRightCommentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "comment id wrong format object id"
                };
            }

            if (string.IsNullOrWhiteSpace(commentUpdate.Comment)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "comment is null or white space"
                };
            }

            UserMetadata userMetadata =
               new UserMetadata().GetUserMetadataFromRequest(Request);

            return postEventHandler.UpdateCommentById(commentId, userMetadata, commentUpdate.Comment);
        }

        [HttpPut]
        [Route("update")]
        public async Task<ResponseBase> UpdateAPost(string postId)
        {
            bool isRightId = ObjectId.TryParse(postId, out var PostId);
            

            var Content = FormData.GetValueByKey("Content");
            if (!isRightId)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "Wrong format"
                };
            }
            HttpFileCollection media = HttpContext.Current.Request.Files;
            if (media != null)
            {
                return await postEventHandler
                .UpdateAPost(PostId, media, Content);
            }
            return await postEventHandler
                .UpdateAPost(PostId, null, Content) ;
        }

        [HttpGet]
        [Route("likes")]
        // Endpoint:/api/v1/post/likes?pid={postid}page=1&size=10&sort=desc [GET]:
        public async Task<ResponseBase> GetLikesOfPostById(string pid, int page, int size) {
            if (string.IsNullOrWhiteSpace(pid)) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "pid is null or white space"
                };
            }

            bool isRightCommentId = ObjectId.TryParse(pid, out var postId);
            if (!isRightCommentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "post id wrong format object id"
                };
            }

            if (page <= 0) page = 0;
            if (size <= 0) size = 1;
            if (size > 20) size = 20;

            return await postEventHandler.GetLikesOfPostById(postId, page, size);
        }

        [HttpPut]
        [Route("likes")] // Endpoint:  /api/v1/post/likes?pid={postid} [POST]:
        public async Task<ResponseBase> LikeAPostByPostId(string pid) {
            bool isRightCommentId = ObjectId.TryParse(pid, out var postId);
            if (!isRightCommentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "post id wrong format object id"
                };
            }
            UserMetadata userMetadata = new UserMetadata().GetUserMetadataFromRequest(Request);

            return await postEventHandler.LikeAPostByPostId(postId, userMetadata);
        }

        [HttpDelete]
        [Route("likes")] // Endpoint:  /api/v1/post/likes?pid={postid}&lid={likeid} [DELETE]:
        public async Task<ResponseBase> UnLikePostByPostId(string pid) {
            bool isRightCommentId = ObjectId.TryParse(pid, out var postId);
            if (!isRightCommentId) {
                return new ResponseBase() {
                    Status = Status.WrongFormat,
                    Message = "post id wrong format object id"
                };
            }
            UserMetadata userMetadata = new UserMetadata().GetUserMetadataFromRequest(Request);

            return await postEventHandler.UnLikeAPostByPostId(postId, userMetadata);
        }
    }
}