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

namespace SocialNetworkBE.Repository
{
    public class PostRespository
    {

        MongoClient Client = new MongoClient("mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true");
        public List<BsonDocument> GetPostByUserId(string userId)
        {
            var DB = Client.GetDatabase("SocialNetwork");

            var collection = DB.GetCollection<BsonDocument>("Post");
            var filter = Builders<BsonDocument>.Filter.Eq("user_Id".ToString(),userId);

            List<BsonDocument> result = collection.Find(filter).ToList();


            return result;
        }
}

    
}
