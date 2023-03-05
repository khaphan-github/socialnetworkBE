using MongoDB.Bson;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.EventHandlers.PostHandler {
    public class PostEventHandler {
        private readonly PostRespository postRespository = new PostRespository();
        public ResponseBase GetPostsWithPaging(int page, int size) {
            // TODO: Get post only not contain like and comment;

            List<Post> listPostWithPaging = postRespository.GetPostByPageAndSizeAndSorted(page, size);

            ResponseBase response = new ResponseBase() {
                Status = Status.Success,
                Message = "Get post success",
                Data = listPostWithPaging
            };

            return response;
        }

        public ResponseBase GetCommentOfPostByPostId(ObjectId postObjectId, int page, int size) {
            return new ResponseBase();
        }
    }
}