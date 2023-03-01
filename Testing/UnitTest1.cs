using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repository;
using System.Linq;
using Newtonsoft.Json.Linq;
using MongoDB.Bson;

namespace Testing {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void Getaccs() {
            AccountResponsitory account = new AccountResponsitory();
            Account accresult = account.GetAccByUsernamePwd("kimkhanh21", "0201hihi");
            string isValid = accresult.Username;
            Assert.IsTrue(isValid.Equals("kimkhanh21"));
        }
        [TestMethod]
        public void GetPosts() {
            PostRespository post = new PostRespository();
            List<Post> postResult = post.GetPostByUserId("ObjectId('63f975f79e8d47050cfa8f19')");
            System.Diagnostics.Debug.WriteLine("result " + postResult.Count + "\n");
            Assert.IsTrue(postResult.Count != 0);
        }

        [TestMethod]
        public void GivenNewPostWhenInsertOnePostThenReturnTrue() {
            PostRespository postRespository = new PostRespository();
            Post post = new Post() {
                Id = new ObjectId(),
            };
            
            bool isInserted = postRespository.CreateNewPost(post);

            Assert.IsTrue(isInserted);
        }
    }
}

