using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;

namespace Testing {

    [TestClass]
    public class AccountRepositoryTest {
        private readonly AccountResponsitory accountResponsitory = new AccountResponsitory();

        [TestMethod]
        public void GivenRightUsername_WhenGetAccountByUsername_ThenReturnAccountInfo() {
            string username = "user-test-1";

            Account accountInserted =
                accountResponsitory.GetAccountByUsername(username);
            Assert.IsNotNull(accountInserted);

            bool isRightUsername = accountInserted.Username.Equals(username);
            Assert.IsTrue(isRightUsername);
        }

        [TestMethod]
        public void GivenAnAccount_WhenInsertAccount_ThenReturnAccountInserted() {
            BCryptService bCryptService = new BCryptService();

            string randomSalt = bCryptService.GetRandomSalt();
            string secretKey = ServerEnvironment.GetServerSecretKey();
            string password = "password-test-1";
            string passwordHash =
                bCryptService
                .HashStringBySHA512(bCryptService.GetHashCode(randomSalt, password, secretKey));

            Account accountTest = new Account() {
                Id = ObjectId.GenerateNewId(),
                DisplayName = "User Test 1",
                Email = "usertest1@gmail.com",
                AvatarUrl = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                UserProfileUrl = "/usertest1",
                Username = "user-test-1",
                Password = passwordHash,
                HashSalt = randomSalt,
            };

            Account accountSaved = accountResponsitory.CreateNewAccount(accountTest);

            Assert.IsNotNull(accountSaved);
            Assert.IsTrue(accountSaved.DisplayName == accountTest.DisplayName);
            Assert.IsTrue(accountSaved.Email == accountTest.Email);
            Assert.IsTrue(accountSaved.AvatarUrl == accountTest.AvatarUrl);
            Assert.IsTrue(accountSaved.UserProfileUrl == accountTest.UserProfileUrl);
            Assert.IsTrue(accountSaved.Username == accountTest.Username);
            Assert.IsTrue(accountSaved.Password == accountTest.Password);
            Assert.IsTrue(accountSaved.HashSalt == accountTest.HashSalt);

            bool isRightPassword = bCryptService.ValidateStringAndHashBySHA512(randomSalt + password + secretKey, accountSaved.Password);
            Assert.IsTrue(isRightPassword);

         

        }
    }
}