﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.ServerConfiguration;


namespace Testing.UnitTesting {
    [TestClass]
    public class EnvironmentTest {
        [TestMethod]
        public void GivenMongoDBConnectionString_WhenGetEnvironmentVariableByKey_ThenReturnRightConnectionString() {
            string mongoDBConnectionString = ServerEnvironment.GetMongoDatabaseConnectionString();
            string expectString = "mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true";
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

            Assert.IsTrue(isEmpty);
            Assert.IsTrue(isRightDatabaseName);
        }

        [TestMethod]
        public void GivenServerSecretKey_WhenGetEnvironmentVariableByKey_ThenReturnRightServerSecretValue() {
            string serverSecretKey = ServerEnvironment.GetServerSecretKey();
            string expectString = "";

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
