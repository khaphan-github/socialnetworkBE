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
            string connectionString = ServerEnvironment.GetMongoDatabaseConnectionString();

            string databaseName = ServerEnvironment.GetMongoDatabaseName();

            MongoClient mongoClient = new MongoClient(connectionString);

            return mongoClient.GetDatabase(databaseName);
        }
    }
}