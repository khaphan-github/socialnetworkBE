using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;

namespace Testing.UnitTesting {
    [TestClass]
    public class BCryptServiceTest {
        private readonly BCryptService BCryptService = new BCryptService();

        [TestMethod] 
        public void GivenSaltPasswordAndSecretkey_WhenGetHashCode_ThenReturnSumOfThem() {
            string salt = BCryptService.GetRandomSalt();
            string password = "user-password-1";
            string secretkey = ServerEnvironment.GetServerSecretKey();

            string expect = salt + password + secretkey;
            string recieve = BCryptService.GetHashCode(salt,password,secretkey);

            Assert.AreEqual(expect, recieve);
        }

        [TestMethod]
        public void GivenPassword_WhenGetHashCodeAndValidate_ThenReturnTrue() {
            string uniqueSalt = BCryptService.GetRandomSalt();
            string password = "user-password-1";
            string secretkey = ServerEnvironment.GetServerSecretKey();

            string textToVerify = BCryptService.GetHashCode(uniqueSalt, password, secretkey);

            string hashCode = BCryptService.HashStringBySHA512(textToVerify);

            bool isMatchHashCode = BCryptService.ValidateStringAndHashBySHA512(textToVerify, hashCode);

            Assert.IsTrue(isMatchHashCode);
        }

        [TestMethod]
        public void GivenPassword_WhenGetHashCodeAndValidate_ThenReturnFalse() {
            string uniqueSalt = BCryptService.GetRandomSalt();
            string password = "user-password-1";
            string secretkey = ServerEnvironment.GetServerSecretKey();

            string textToVerify = BCryptService.GetHashCode(uniqueSalt, password, secretkey);

            string hashCode = BCryptService.HashStringBySHA512(textToVerify);

            bool isMatchHashCode = BCryptService.ValidateStringAndHashBySHA512(textToVerify + "bug", hashCode);

            Assert.IsFalse(isMatchHashCode);
        }
    }
}
