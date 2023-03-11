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
            CommentRepository commentRepository = new CommentRepository();
            Comment createdComment = commentRepository.CreateCommentAPost(comment);
            Assert.IsNotNull(createdComment);
        }

        [TestMethod]
        public void GivenCommentWithParent_WhenCreateComment_ThenReturnCommentCreated() {
            Comment comment = CreateTestComment("640ca1d672055cf98c39b291");
            CommentRepository commentRepository = new CommentRepository();
            Comment createdComment = commentRepository.CreateCommentAPost(comment);
            Assert.IsNotNull(createdComment);
        }
    }
}
