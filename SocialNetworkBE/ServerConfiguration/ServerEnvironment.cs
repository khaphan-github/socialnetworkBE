using System.Configuration;

namespace SocialNetworkBE.ServerConfiguration {
    public static class ServerEnvironment {
        public static string GetMongoDatabaseConnectionString () {
            string key = "MongoDBConnectionString-KimKhanhCluster";
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetMongoDatabaseName () {
            string key = "MongoDBConnectionString-KimKhanhCluster-DatabaseName";
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetServerSecretKey () {
            string key = "API-Server-SecretKey";
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetRedisCacheHost() {
            string key = "Redis-Cache-Docker-Host";
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetRedisCachePort () {
            string key = "Redis-Cache-Docker-Port";
            return ConfigurationManager.AppSettings[key];
        }
    }
}