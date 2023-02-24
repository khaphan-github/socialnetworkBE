using MongoDB.Driver;
using System.Configuration;

namespace SocialNetworkBE.Repository.Config {
    public class MongoDBConfiguration {
        private readonly IMongoDatabase IMongoDatabase;

        public MongoDBConfiguration()
        {
            IMongoDatabase = GetMongoDatabase();
        }

        public IMongoDatabase GetMongoDB()
        {
            return IMongoDatabase;
        }

        private IMongoDatabase GetMongoDatabase()
        {
            string connectionString = ConfigurationManager.AppSettings["connectionString"];

            string databaseName = ConfigurationManager.AppSettings["databaseName"];

            MongoClient mongoClient = new MongoClient(connectionString);

            return mongoClient.GetDatabase(databaseName);
        }
    }
}