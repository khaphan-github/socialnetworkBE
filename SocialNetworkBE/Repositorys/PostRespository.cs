using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;
using MongoDB.Driver.Builders;
using System.Web.Mvc;
using MongoDB.Bson.Serialization;
using WebGrease.Css.Extensions;
using MongoDB.Bson.IO;

namespace SocialNetworkBE.Repository
{
    public class PostRespository
    {

        MongoClient Client = new MongoClient("mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true");
        public List<Post> GetPostByUserId(string userId)
        {
            var DB = Client.GetDatabase("SocialNetwork");

            var collection = DB.GetCollection<BsonDocument>("Post");
            var filter = Builders<BsonDocument>.Filter.Eq("user_Id".ToString(),userId);


            List<BsonDocument> results = collection.Find(filter).ToList();
            List<Post> resultPost = results
                .Select(post => new SocialNetworkBE.Repositorys.DataModels.Post
                {
                    Id = post["_id"].AsObjectId,
                    Content = post["content"].AsString,
                    UserId = post["user_Id"].AsString,
                    Comments = post["comments"].AsBsonArray,
                }).Where(x => x.UserId.ToString() == userId).ToList();
            return resultPost;
        }
    }

    
}
