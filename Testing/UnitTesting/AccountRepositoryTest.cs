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

        private Account CreateAccountTest(string username, string password) {
            BCryptService bCryptService = new BCryptService();

            string randomSalt = bCryptService.GetRandomSalt();
            string secretKey = ServerEnvironment.GetServerSecretKey();
            string displayName = "Kha Phan";
            string email = "khaphanne@gmail.com";
            string profile = "/khaphanne";

            string passwordHash =
                bCryptService
                .HashStringBySHA512(bCryptService.GetHashCode(randomSalt, password, secretKey));

            Account accountTest = new Account() {
                Id = ObjectId.GenerateNewId(),
                DisplayName = displayName,
                Email = email,
                AvatarUrl = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                UserProfileUrl = profile,
                Username = username,
                Password = passwordHash,
                HashSalt = randomSalt,
            };
            return  accountResponsitory.CreateNewAccount(accountTest);
        }

        [TestMethod]
        [DataRow("username-test-1", "password-test-1")]
        [DataRow("username-test-2", "password-test-1")]
        public void GivenRightUsername_WhenGetAccountByUsername_ThenReturnAccountInfo(string username, string password) {
            Account accountTest = CreateAccountTest(username, password);

            Account accountInserted =
                accountResponsitory.GetAccountByUsername(accountTest.Username);

            Assert.IsNotNull(accountInserted);
            bool isRightUsername = accountInserted.Username.Equals(username);
            Assert.IsTrue(isRightUsername);

            bool isDeleted = accountResponsitory.DeleteAccount(accountTest.Id);
            Assert.IsTrue(isDeleted);
        }
    }
}