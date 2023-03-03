using MongoDB.Driver;
using SocialNetworkBE.ServerConfiguration;
using System.Configuration;

namespace SocialNetworkBE.Repository.Config {
    public class MongoDBConfiguration {
        private readonly IMongoDatabase IMongoDatabase;

        public MongoDBConfiguration()
        {
            IMongoDatabase = GetMongoDatabase();
        }

        public IMongoDatabase GetMongoDBConnected()
        {
            return IMongoDatabase;
        }

        private IMongoDatabase GetMongoDatabase()
        {
            string connectionString = Environment.GetMongoDatabaseConnectionString();

            string databaseName = Environment.GetMongoDatabaseName();

            MongoClient mongoClient = new MongoClient(connectionString);

            return mongoClient.GetDatabase(databaseName);
        }
    }
}