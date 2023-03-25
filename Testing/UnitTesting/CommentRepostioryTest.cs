using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.UnitTesting {
    [TestClass]
    public class CommentRepostioryTest {
        /** 
        private readonly CommentRepository commentRepository = new CommentRepository();

        private Comment CreateTestComment() {
            return new Comment() {
                Id = ObjectId.GenerateNewId(),
                ActionCount = 0,
                CommentCount = 0,
                Content = "Nhưng cớ sao mình chẵng nói nhau câu gì...",
                PostId = ObjectId.Parse("640c9c91ff4bb9be2af7a88f"),
                CreateDate = DateTime.Now,
                OwnerId = ObjectId.Parse("640c2418220bc59472d04157"),
                OwnerAvatarURL = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                OwnerDisplayName = "Anh Hùng Xạ Điêu",
                OwnerProfileURL = "/anhhungxadieu"
            };
        }

        private Comment CreateTestComment(string parentId) {
            return new Comment() {
                Id = ObjectId.GenerateNewId(),
                ActionCount = 0,
                CommentCount = 0,
                ParentId = ObjectId.Parse(parentId),
                Content = "Nhưng cớ sao mình chẵng nói nhau câu gì...",
                PostId = ObjectId.Parse("640c9c91ff4bb9be2af7a88f"),
                CreateDate = DateTime.Now,
                OwnerId = ObjectId.Parse("640c2418220bc59472d04157"),
                OwnerAvatarURL = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                OwnerDisplayName = "Anh Hùng Xạ Điêu",
                OwnerProfileURL = "/anhhungxadieu"
            };
        }
        [TestMethod]
        public void GivenCommentNoParent_WhenCreateComment_ThenReturnCommentCreated() {
            Comment comment = CreateTestComment();
            Comment createdComment = commentRepository.CreateCommentAPost(comment);

            Assert.IsNotNull(createdComment);

        }

        [TestMethod]
        public void GivenCommentWithParent_WhenCreateComment_ThenReturnCommentCreated() {
        Comment parentComment = CreateTestComment();
            Comment childComment = CreateTestComment(parentComment.Id.ToString());

            Comment createdComment = commentRepository.CreateCommentAPost(childComment);

            Assert.IsNotNull(createdComment);

      
        }

        [TestMethod]
        public void GivenCommentId_WhenDeteteCommentById_ThenReturnSuccess() {
            Comment comment = CreateTestComment();
            commentRepository.CreateCommentAPost(comment);


      
        }

        [TestMethod]
        public void GivenCommentContent_WhenUpdateComment_ThenReturnSuccess() {
            Comment comment = CreateTestComment();
            commentRepository.CreateCommentAPost(comment);
            string contentToUpdate = "Vì ngày hôm nay em cưới rồi, mai sau anh sống thế nào...";


         

        }

        [TestMethod]
        [DataRow(0, 1)]
        [DataRow(1, 1)]

        [DataRow(0, 10)]
        [DataRow(1, 5)]

        public void GivenPostId_WhenGetCommentByPostId_ThenReturnListComments(int page, int size) {
            ObjectId postId = ObjectId.Parse("640c9c91ff4bb9be2af7a88f");
            List<Comment> comments = commentRepository.GetCommentOfPostWithPaging(postId, page, size);
            Assert.IsNotNull(comments);
            Assert.IsTrue(comments.Count == size);
        }
        */
    }
}
