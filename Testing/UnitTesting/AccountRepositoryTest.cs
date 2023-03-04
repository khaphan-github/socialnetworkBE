using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;

namespace Testing {
    /// <summary>
    /// Summary description for AccountRepositoryTest
    /// </summary>
    [TestClass]
    public class AccountRepositoryTest {
        private readonly AccountResponsitory accountResponsitory = new AccountResponsitory();

        [TestMethod]
        public void GivenRightUsernameAndPassword_WhenGetAccountByUsernameAndPassword_ThenReturnAccountInfo() {
            string username = "user-test-0";
            string password = "password-test-0";

            Account accountInserted =
                accountResponsitory.GetAccountByUsernameAndPassword(username, password);
            Assert.IsNotNull(accountInserted);

            bool isRightUsername = accountInserted.Username.Equals(username);
            Assert.IsTrue(isRightUsername);

            bool isRightPassword = accountInserted.Password.Equals(password);
            Assert.IsTrue(isRightPassword);
        }

        [TestMethod]
        public void GivenEmptyUsernameAndRightPassword_WhenGetAccountByUsernameAndPassword_ThenRerturnNull() {
            string username = "";
            string password = "password-test-not-exist";
           
            Account accountInserted =
                accountResponsitory.GetAccountByUsernameAndPassword(username, password);
            
            Assert.IsNull(accountInserted);
        }

        [TestMethod]
        public void GivenRightUsernameAndEmptyPassword_WhenGetAccountByUsernameAndPassword_ThenRerturnNull() {
            string username = "user-test-0";
            string password = "";

            Account accountInserted =
                accountResponsitory.GetAccountByUsernameAndPassword(username, password);

            Assert.IsNull(accountInserted);
        }

        [TestMethod]
        public void GivenWrongUsernameAndPassword_WhenGetAccountByUsernameAndPassword_ThenRerturnNull () {
            string username = "user-test-not-exist";
            string password = "password-test-not-exist";

            Account accountInserted =
                accountResponsitory.GetAccountByUsernameAndPassword(username, password);
            Assert.IsNull(accountInserted);
        }

        [TestMethod]
        public void GivenWrongUsernameAndRightPassword_WhenGetAccountByUsernameAndPassword_ThenRerturnNull() {
            string username = "user-test-not-exist";
            string password = "password-test-0";

            Account accountInserted =
                accountResponsitory.GetAccountByUsernameAndPassword(username, password);
            Assert.IsNull(accountInserted);
        }

        [TestMethod]
        public void GivenRightUsernameAndWrongPassword_WhenGetAccountByUsernameAndPassword_ThenRerturnNull() {
            string username = "user-test-0";
            string password = "password-test-not-exist";

            Account accountInserted =
                accountResponsitory.GetAccountByUsernameAndPassword(username, password);

            Assert.IsNull(accountInserted);
        }

        [TestMethod]
        public void GivenAnAccount_WhenInsertAccount_ThenReturnAccountInserted() {
            Account accountTest = new Account() {
                Id = ObjectId.GenerateNewId(),
                DisplayName = "User Test 1",
                Email = "usertest1@gmail.com",
                AvatarUrl = "https://s120-ava-talk.zadn.vn/d/9/d/5/17/120/4123221a970ff46aff6e24ef54e0fa1f.jpg",
                UserProfileUrl = "/usertest1",
                Username = "user-test-1",
                Password = "password-test-1"
            };

            Account accountSaved = accountResponsitory.CreateNewAccount(accountTest);
            Assert.IsNotNull(accountSaved);

            bool isDeleted = accountResponsitory.DeleteAccount(accountTest.Id);
            Assert.IsTrue(isDeleted);
        }
    }
}