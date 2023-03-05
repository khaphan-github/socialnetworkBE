using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using System.Collections.Generic;
using System.Linq;

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

        public ResponseBase GetCommentOfPostByPostId(ObjectId postObjectId, int page, int size) {
            return new ResponseBase();
        }
    }
}