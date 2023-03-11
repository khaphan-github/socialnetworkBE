using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repository;
using MongoDB.Bson;

namespace Testing {
    [TestClass]
    public class PostRepositoryTest {
        private readonly PostRespository PostRespository = new PostRespository();



        [TestMethod]
        public void GivenUserObjectId_WhenGetPostByUserId_ThenReturnListOfPost() {

            ObjectId existedUserObjectId = ObjectId.Parse("64049464be304cff9d2062be");

            List<Post> postResult = PostRespository.GetPostByUserId(existedUserObjectId);

            Assert.IsNotNull(postResult);
        }

        [TestMethod]
        public void GivenNewPost_WhenInsertOnePost_ThenReturnRightPost() {
            // Given
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

            Post createdPost = PostRespository.CreateNewPost(post);

            bool isEmptyPost = createdPost == null;
            Assert.IsTrue(!isEmptyPost);
            // When

            Post savedPost = PostRespository.GetPostById(post.Id);
            // Then

            bool isRightId = savedPost.Id == post.Id;
            Assert.IsTrue(isRightId);

            bool isRightMedia0 = savedPost.Media[0] == post.Media[0];
            Assert.IsTrue(isRightMedia0);

            bool isRightMedia1 = savedPost.Media[1] == post.Media[1];
            Assert.IsTrue(isRightMedia1);

            bool isNumOfCommentEqualZero = savedPost.NumOfComment == 0;
            Assert.IsTrue(isNumOfCommentEqualZero);

            bool isRightUserId = savedPost.OwnerId.Equals(post.OwnerId);
            Assert.IsTrue(isRightUserId);

            bool isRightContent = savedPost.Content == post.Content;
            Assert.IsTrue(isRightContent);
        }      
    }
}

