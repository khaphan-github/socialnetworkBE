using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repository;
using System.Linq;

namespace Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetAllComments()
        {
            var testComment = GetTestComments();
            var controller = new CommentController(testComment);

            var result = controller.GetAllProducts() as List<Comment>;
            Assert.AreEqual(testComment.Count, result.Count);
        }
        [TestMethod]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            var testComment = GetTestComments();
            var controller = new CommentController(testComment);

            var result = await controller.GetAllProductsAsync() as List<Comment>;
            Assert.AreEqual(testComment.Count, result.Count);
        }

        private List<Comment> GetTestComments()
        {
            commentRepository comments = new commentRepository();
            IEnumerable<Comment>testProducts = comments.GetComments();

            return testProducts.ToList();
        }
    }
}
