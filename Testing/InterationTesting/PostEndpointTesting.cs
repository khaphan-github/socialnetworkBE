using DnsClient.Protocol;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Controllers;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;

namespace Testing.InterationTesting {
    [TestClass]
    public class PostEndpointTesting {
        private readonly PostRespository PostRespository = new PostRespository();

        private Post CreatePostToTest(int numOfComment) {
            ObjectId ownerId = ObjectId.Parse("64049464be304cff9d2062be");

            string content = "Gavin :\r\nHẹn em vào một ngày chiều thu / khi mùa hoa kia dần chớm nở\r\nTa lại " +
                "hòa mình vào bài ca / là lúc tim em đã dần hé mở \r\nAnh đào chuyển mình xơ xác đông phong / " +
                "đôi mắt em mang màu nắng  \r\nNối sợi duyên tình đã đứt lâu năm / chẳng có ai thua và không người" +
                " thắng\r\nNhư tấm chân tình kia rất mong manh / bến đổ bên kia chân trời đang thôi thúc \r\nHọa" +
                " vân trong ánh mắt người / anh chỉ muốn ngắm nhìn đôi chút \r\nLà lúc mà anh ghét vội nhánh anh " +
                "đào tại sao lại nỡ đơm hoa vội vàng \r\nBên bến sông buồn đã phủ rêu phong mái tóc em vẫn chưa kịp" +
                " xõa \r\nBỉ ngạn phủ kín đường về\r\nBuông mình theo ánh trăng trôi \r\nLẽ loi bước tiếp độc hành" +
                " \r\nMây mờ phủ kín trăm nơi \r\nLá chẳng xanh mãi một màu / thử hỏi duyên tình bao lâu ? \r\nĐến " +
                "khi mái tóc bạc màu / Giờ còn lại gì trao nhau \r\nRừng phong , mắt ngọc trông nàng \r\nThủy thanh ," +
                " trùng non bất phùng lai\r\nTrống không , Tâm tư với Bông vàng \r\nSuy nghĩ , rối bời khóc Cùng ai x2";

            ObjectId postId = ObjectId.GenerateNewId();
            Post post = new Post() {
                Id = postId,
                Media = new List<string>() {
                    "https://images.unsplash.com/photo-1503023345310-bd7c1de61c7d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY" +
                    "2h8Mnx8aHVtYW58ZW58MHx8MHx8&w=1000&q=80",
                    "https://images.unsplash.com/photo-1503023345310-bd7c1de61c7d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY" +
                    "2h8Mnx8aHVtYW58ZW58MHx8MHx8&w=1000&q=80"
                },
                OwnerProfileURL = "/usertest1",
                OwnerDisplayName = "User Test 1",
                OwnerAvatarURL = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                CreateDate = DateTime.Now,
                UpdateAt = DateTime.Now,

                NumOfComment = 0,
                Scope = "public",
                OwnerId = ownerId,
                Content = content,
                CommentsURL = "/api/v1/post/comments/?pid=" + postId.ToString() + "&page=0&size=3",
                LikesURL = "/api/v1/post/likes/?pid=" + postId.ToString() + "&page=0&size=3",
            };
            List<Comment> comments = new List<Comment>();

            for (int i = 0; i < numOfComment; i++) {
                comments.Add(new Comment() {
                    OwnerProfileURL = "/usertest1",
                    OwnerDisplayName = "User Test 1",
                    OwnerAvatarURL = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                    Content = "Nice: " + i.ToString(),
                    CreateDate = DateTime.Now,
                    NumOfLike = 0,
                    OwnerId = ownerId,

                });
            }
            post.Comments = comments;

            post.Likes = new List<Like> {
                {
                    new Like() {
                          OwnerProfileURL = "/usertest1",
                        OwnerDisplayName = "User Test 1",
                        OwnerAvatarURL = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                        CreateDate= DateTime.Now,
                        OwnerId = ownerId,

                        TypeofAction = "Like"
                    }
                }

            };

            return PostRespository.CreateNewPost(post);
        }
       
        public ResponseBase GetResponseBaseWhenDeletePost(string id) {
            PostController postController = new PostController();
            ResponseBase response = postController.DetetePostById(id.ToString());
            return response;
        }

        /**
         * Test Get Post With Paging
         */
        [TestMethod]
        [DataRow(0, 0)]
        [DataRow(0, 1)]
        [DataRow(0, 19)]
        [DataRow(0, 20)]
        [DataRow(0, 21)]
        [DataRow(0, 100)]

        [DataRow(1, 0)]
        [DataRow(1, 1)]
        [DataRow(1, 19)]
        [DataRow(1, 20)]
        [DataRow(1, 21)]
        [DataRow(1, 100)]
        public void GivenPageAndSize_WhenGetCurrentPost_ThenReceivePostWithPaging(int page, int size) {
            PostController postController = new PostController();

            ResponseBase response = postController.GetPostListWithPaging(page, size);
            Assert.IsNotNull(response);

            Assert.IsTrue(response.Status == Status.Success);
            Assert.IsTrue(response.Message == "Get post success");

            PagingResponse pagingResponse = response.Data as PagingResponse;
            Assert.IsNotNull(pagingResponse);

            List<PostResponse> postResponse = pagingResponse.Paging as List<PostResponse>;
            Assert.IsNotNull(postResponse);
            Assert.IsTrue(postResponse.Count <= 22);
            Assert.IsTrue(postResponse.Count >= 1);
        }

        [TestMethod]
        [DataRow(999999999, 1)]
        [DataRow(10000, 1)]
        public void GivenLargePageIndex_WhenGetCurrentPost_ThenRecieveEmptyPost(int page, int size) {
            PostController postController = new PostController();

            ResponseBase response = postController.GetPostListWithPaging(page, size);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == Status.Failure);
            Assert.IsTrue(response.Message == "Empty post");

            PagingResponse pagingResponse = response.Data as PagingResponse;
            Assert.IsNull(pagingResponse);
        }

        /**
        * Test Delete Post By Id 
        */
        [TestMethod]
        public void GivenPostId_WhenDetetePostById_ThenRecieveSuccessResponse() {
            Post postToDelete = CreatePostToTest(0);
            ResponseBase response = GetResponseBaseWhenDeletePost(postToDelete.Id.ToString());
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == Status.Success);
            Assert.IsTrue(response.Message == "Detete success");
        }

        [TestMethod]
        [DataRow("6406c3424218e4f142798669")]
        [DataRow("6406c3424218e4f142798668")]
        public void GivenNotExistPostId_WhenDeletePostById_ThenRecieveFailueResponse(string id) {
            PostController postController = new PostController();
            ResponseBase response = postController.DetetePostById(id);
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == Status.Failure);
            Assert.IsTrue(response.Message == "Delete failure");
        }

        [TestMethod]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("6406c3424218e4f---------")]
        [DataRow("6406c3424218e4f142798663999999")]
        [DataRow("***")]
        public void GivenWrongPostIdFormat_WhenDeletePostById_ThenRecieveWrongFormatMessage(string id) {
            ResponseBase response = GetResponseBaseWhenDeletePost(id.ToString());

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Status == Status.WrongFormat);
            Assert.IsTrue(response.Message == "ObjectId Wrong Format");
        }

        /** 
            Test get comment of post by id
         */
        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(0, 10)]

        [DataRow(0, 5)]
        [DataRow(1, 5)]

        [DataRow(0, 2)]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        [DataRow(3, 2)]
        [DataRow(4, 2)]
        public void GivenPostId_WhenGetCommentOfPostByPostId_ThenRecieveComments(int page, int size) {
            Post postToTest = CreatePostToTest(10);
            PostController postController = new PostController();

            ResponseBase response = postController.GetCommentOfPostById(postToTest.Id.ToString(), page, size);
            Assert.IsNotNull(response);

            Assert.IsTrue(response.Status == Status.Success);
            Assert.IsTrue(response.Message == "Get comment success");

            List<Comment> comments = response.Data as List<Comment>;
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Count == size);
                        
            bool isDeleted = PostRespository.DetetePostById(postToTest.Id);
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void GivenPostIdWithEmptyComment_WhenGetCommentOfPostByPostId_ThenRecieveNoComment() {
            Post postWithEmptyComment = CreatePostToTest(0);
            
            PostController postController = new PostController();
            
            ResponseBase response = 
                postController.GetCommentOfPostById(postWithEmptyComment.Id.ToString(), 0, 1);

            Assert.IsNotNull(response);
            
            Assert.IsTrue(response.Status == Status.Failure); 
            Assert.IsTrue(response.Message == "This post have no comment");

            List<Comment> comments = response.Data as List<Comment>;
            Assert.IsNull(comments);

            bool isDeleted = PostRespository.DetetePostById(postWithEmptyComment.Id);
            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        [DataRow(" ")]
        [DataRow("6406c3424218e4f---------")]
        [DataRow("6406c3424218e4f142798663999999")]
        [DataRow("***")]
        public void GivenWrongformatParam_WhenGetCommentOfPostByPostId_ThenRecieveWrongFormat(string pid) {

            PostController postController = new PostController();

            ResponseBase response =
                postController.GetCommentOfPostById(pid, 0, 1);

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Status == Status.WrongFormat);
            Assert.IsTrue(response.Message == "ObjectId Wrong Format");
        }

        [TestMethod]
        public void GivenEmptyPagram_WhenGetCommentOfPostByPostId_ThenRecieveWrongFormat() {
            PostController postController = new PostController();
            ResponseBase response =
                postController.GetCommentOfPostById("", 0, 0);

            Assert.IsNotNull(response);

            Assert.IsTrue(response.Status == Status.WrongFormat);
            Assert.IsTrue(response.Message == "This request require pid, page , size");
        }
    }
}