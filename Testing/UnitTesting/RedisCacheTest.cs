using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Services.RedisCache;
using System;
using System.Collections.Generic;

namespace Testing {

    [TestClass]
    public class RedisCacheTest {
        private readonly RedisCacheService redisCacheService = new RedisCacheService();

        [TestMethod]
        public void GivenObject_WhenSetPostToCache_ThenGetObjectIsNotNull() {
            string key = "thisiskey";
            // Given
            ObjectId ownerId = ObjectId.Parse("63f975f79e8d47050cfa8f19");

            string content = "Gavin :\r\nHẹn em vào một ngày chiều thu / khi mùa hoa kia dần chớm nở\r\nTa lại ";

            Post post = new Post() {
                Id = ObjectId.GenerateNewId(),
                Media = new List<string>() {
                    "https://images.unsplash.com/photo-1503023345310-bd7c1de61c7d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY" +
                    "2h8Mnx8aHVtYW58ZW58MHx8MHx8&w=1000&q=80",
                    "https://images.unsplash.com/photo-1503023345310-bd7c1de61c7d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxzZWFyY" +
                    "2h8Mnx8aHVtYW58ZW58MHx8MHx8&w=1000&q=80"
                },
                CreateDate = DateTime.Now,
                UpdateAt = DateTime.Now,
                NumOfComment = 0,
                Scope = "public",
                OwnerId = ownerId,
                Content = content,
            };

            string commentGuid = Guid.NewGuid().ToString();
            post.Comments = new Dictionary<string, Comment> {
                {
                    commentGuid,
                    new Comment() {
                        Content = "Nice",
                        CreateDate = DateTime.Now,
                        NumOfLike = 0,
                        OwnerId = ownerId,

                    }
                }
            };
            string likeGuid = Guid.NewGuid().ToString();
            post.Likes = new Dictionary<string, Like> {
                {
                    likeGuid,
                    new Like() {
                        CreateDate= DateTime.Now,
                        OwnerId = ownerId,
                        TypeofAction = "Like"
                    }
                }

            };
            redisCacheService.SetObjectToCache(key, post);
            string jsonPostFromCache = redisCacheService.GetJsonObjectFromCache(key);
            Assert.IsNotNull(jsonPostFromCache);
        }
    }
}
