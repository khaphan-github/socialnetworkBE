using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.ServerConfiguration;


namespace Testing.UnitTesting {
    [TestClass]
    public class EnvironmentTest {
        [TestMethod]
        public void GivenMongoDBConnectionString_WhenGetEnvironmentVariableByKey_ThenReturnRightConnectionString() {
            string mongoDBConnectionString = ServerEnvironment.GetMongoDatabaseConnectionString();
            string expectString = "mongodb+srv://kkneee0201hihi:socialnetwork123@cluster0.idnnc8t.mongodb.net/?retryWrites=true";
            bool isEmpty = mongoDBConnectionString == null;
            bool isRightConnectionString = mongoDBConnectionString == expectString;

            Assert.IsTrue(!isEmpty);
            Assert.IsTrue(isRightConnectionString);
        }
        [TestMethod]
        public void GivenMongoDBDatabaseKey_WhenGetEnvironmentVariableByKey_ThenReturnRightDatabaseName() {
            string mongoDatabaseName = ServerEnvironment.GetMongoDatabaseName();
            string expectString = "SocialNetwork";

            bool isEmpty = mongoDatabaseName == null;
            bool isRightDatabaseName = mongoDatabaseName == expectString;

            Assert.IsTrue(!isEmpty);
            Assert.IsTrue(isRightDatabaseName);
        }

        [TestMethod]
        public void GivenServerSecretKey_WhenGetEnvironmentVariableByKey_ThenReturnRightServerSecretValue() {
            string serverSecretKey = ServerEnvironment.GetServerSecretKey();
            string expectString = "-----BEGIN RSA PRIVATE KEY-----\r\nMIIEpAIBAAKCAQEA33pP0+dfZeMHv5AW12yRtZntxtGX8wQ5pfrxdx98XqcBIDtk\r\nQkOKztVUuxXBsLWG+1k9q0elzK+jRyTKIJPw8SD5s/eDjI8WvDVnOmzYKFlNLa1w\r\nSC6VAtrqJy7XFdugZ4Snh48QumLje49XV5RTDRxSjG2u8jQwPkLJdPwShEgbZZn2\r\nWMF55Wb5Us/iFeKf9SgMgFn8Q81aRKfxZz7QxTE+hxBc0bdwBVzgYV6JaFk4ninS\r\ntN2TgxHPPfvDTG/WofZ2N4Jc+UU0P7n+obxM8KPpRO65FH+kDbABVTSOSIB10J0E\r\nRXo6jbhPG9+AWwCUzRm6W8Raa2c8ywcZXFmNnQIDAQABAoIBADBx9GIscxrEN4bI\r\ndpsmlwO2JbyG7RiqXtDjcHrxYWWncHALT/WpbKrfxil0UvO6tlNAikTaGFAh9xRS\r\nHlsnlwC+tELWMjdDQZt1PDaHJ+i/SyLOyVtsdbMTv9TUq526abBnKmYRln/BNi0v\r\n6rA6oDJkZGS61kT9GUQ1+DQa/OydyT7TYGQ95Bh0e9hFQeE2qzfPTUEEcmLxg9u/\r\nSC5gFzB6K7BTYxOJvxF8PX7Q6HpfCXW8k4Umciwdm97EQE2+eNJezlPhZmfpIZvb\r\nnFN7ReFlLthtXKK+SY2sOYzI75Lg/JYzUQk0tqp71sy2KMBGjC9FHk/5CAMahaBK\r\nkgl5cAECgYEA+ZBL36TqjsFTQcRZ6Pj+oZIzrvDuqg8pqYbWDFTc4KeWc3o+TYui\r\nXMRiEeY/x4ZWUPXwowpRoZS3Hvwyohqldd8NfcqCFCGdToglXYB34XKkxLHPTSoM\r\n1xnZ9J747873hYxLzGce8xVxjdlahvOyVLf198dCqDfX6BlFxYYG/AECgYEA5T3J\r\npJx7P5K0eszFCf0KZy2XD3oW+8vOE0jDm1fS8dpwtxNlbqIXjQrBDhskZGAw7eC7\r\nMNMJXxmM6JQYd6ISoAepCx4QQzJfjv2QQ/NX5GhWGX/26b4at4vZz8nY3eyKu2m1\r\ngmMOGtm7zZfXbBIIdA2xOBBv5LBjS+wduXcVAZ0CgYEA1LXDpcxXy/NFdlHYXHUV\r\n+zq06JAcrEAeP4tYl8whMy5EUwrHXar9aVpNSHpatEJxAbKONzkNWM57wmIs+fQC\r\nQVQrmKLpeiUogg4S4v4jr7nabHgte9SyewhiuFhjT2q1mFN7K65dN814KvIuccX2\r\norTlZhqlWuG2GfKogHH7NAECgYEAti5DeNmPLTe2zLy9frGlOdhGa9tYNqWCUVsL\r\ncQH7t48k8qmse56/3qwEPuSdKFQ1rmZh/WxJz1Ur4Y4IX9LiwGE2G4q5291FigrD\r\nQu06FWBBxKQooxwceW/gGr1L5xpcKpLY2BVGmVoeYUZhwhYWt7xF8ctGUVVIpIay\r\nHLmhbRUCgYBWWljqPoeMDoPbkVljSeexx22hnRPAWZr566TX9QzRDC6++38yRXCt\r\naFVNRhrC3796qDl8kuYjjkdBT+L1Bf7tsf19lfR+Fcz4Od2U2xYr4MMNsei4JLJR\r\nm94rf3zdXmGCAPYFwOBobnod7uz4jHYj3D3M6Mp6gBI+9fFfCKN/5A==\r\n-----END RSA PRIVATE KEY-----";

            bool isEmpty = serverSecretKey == null;
            bool isRightSecretKey = serverSecretKey == expectString;

            Assert.IsTrue(!isEmpty);
            Assert.IsTrue(isRightSecretKey);
        }

        [TestMethod]
        public void GivenRedisHost_WhenGetEnvironmentVariableByKey_ThenReturnRightRedisHostValue() {
            string redisHostValue = ServerEnvironment.GetRedisCacheHost();
            string expectString = "localhost";

            bool isEmpty = redisHostValue == null;
            bool isRightRedisHostValue = redisHostValue == expectString;

            Assert.IsTrue(!isEmpty);
            Assert.IsTrue(isRightRedisHostValue);
        }

        [TestMethod]
        public void GivenRedisPort_WhenGetEnvironmentVariableByKey_ThenReturnRightRedisPortValue() {
            int redisPortValue = int.Parse(ServerEnvironment.GetRedisCachePort());
            int expectInt = 8080;
            bool isRightRedisHostValue = redisPortValue == expectInt;
            Assert.IsTrue(isRightRedisHostValue);
        }
    }
}
