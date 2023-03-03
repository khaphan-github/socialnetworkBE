using MongoDB.Driver;
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
            //  ConfigurationManager.AppSettings["connectionString"]
            //  ConfigurationManager.AppSettings["databaseName"]
            string connectionString =  "mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true";

            string databaseName = "SocialNetwork";

            MongoClient mongoClient = new MongoClient(connectionString);

            return mongoClient.GetDatabase(databaseName);
        }
    }
}