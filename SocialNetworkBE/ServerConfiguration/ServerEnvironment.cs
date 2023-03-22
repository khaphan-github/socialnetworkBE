using System.Configuration;

namespace SocialNetworkBE.ServerConfiguration {
    public static class ServerEnvironment {
        public static string GetMongoDatabaseConnectionString () {
            string key = "MongoDBConnectionString-SocialCluster";
            System.Diagnostics.Debug.WriteLine (ConfigurationManager.AppSettings[key]);
            return ConfigurationManager.AppSettings[key];
        }

        public static string GetMongoDatabaseName () {
            string key = "MongoDBConnectionString-SocialCluster-DatabaseName";
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

        public static string GetFirebaseApiKey()
        {
            string key = "FirebaseApiKey";
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetFirebaseBucket()
        {
            string key = "FirebaseBucket";
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetFirebaseAuthEmail()
        {
            string key = "FirebaseAuthEmail";
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetFirebaseAuthPwd()
        {
            string key = "FirebaseAuthPwd";
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetFirebaseStorageDomain() {
            string key = "FirebaseStorageDomain";
            return ConfigurationManager.AppSettings[key];
        }
        public static string GetClientCORSDomain() {
            string key = "Client-CORS-Domain";
            return ConfigurationManager.AppSettings[key];

        }
    }
}