using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Linq;


namespace SocialNetworkBE.Repository {
    public class PostRespository {
        private const string PostDocumentName = "Post";

        private readonly MongoClient Client = new MongoClient("mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true");

        public List<Post> GetPostByUserId(string userId) {

            var DB = Client.GetDatabase("SocialNetwork");

            var collection = DB.GetCollection<BsonDocument>(PostDocumentName);

            var filter = Builders<BsonDocument>.Filter.Eq("user_Id".ToString(), userId);


            List<BsonDocument> results = collection.Find(filter).ToList();

            List<Post> resultPost =
                results.Select(post => new Post {
                    Id = post["_id"].AsObjectId,
                    Content = post["content"].AsString,
                    UserId = post["user_Id"].AsString,
                    Comments = (IDictionary<Guid, Comment>)post["comments"],
                }).Where(x => x.UserId.ToString() == userId).ToList();
            return resultPost;
        }

        public bool CreateNewPost(Post NewPost) {

            try {
                var DB = Client.GetDatabase("SocialNetwork");

                IMongoCollection<Post> postDocument = DB.GetCollection<Post>(PostDocumentName);

                postDocument.InsertOne(NewPost);

                return true;

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return false;
            }

        }
    }


}
