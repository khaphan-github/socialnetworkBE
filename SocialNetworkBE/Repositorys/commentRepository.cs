using SocialNetworkBE.Repositorys.DataModels;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkBE.Repository
{
    public class commentRepository
    {

       MongoClient Client = new MongoClient(ConfigurationManager.AppSettings["connectionString"]);

        public IEnumerable<Comment> GetComments()
        {
            var DB = Client.GetDatabase("sample_mflix");

            var collection = DB.GetCollection<BsonDocument>("comments");

            var allComments = collection.Find(new BsonDocument()).ToList();

            IEnumerable<SocialNetworkBE.Repositorys.DataModels.Comment> comments = allComments
                .Select(bd => new SocialNetworkBE.Repositorys.DataModels.Comment
                {
                    Id = bd["_id"].AsObjectId,
                    email = bd["email"].AsString,
                    text = bd["text"].AsString,
                    date = bd["date"].AsDateTime,
                    name = bd["name"].AsString,
                    movie_id = bd["movie_id"].AsObjectId

                });
            return comments;
        }
    }
}
