using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.Interfaces;
using System;
using System.Linq;

namespace SocialNetworkBE.Repository {
    public class AccountResponsitory : IAccountRepository {
        private IMongoCollection<Account> AccountCollection { get; set; }

        public AccountResponsitory() {
            const string AccountDocumentName = "Account";

            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            IMongoDatabase databaseConnected = MongoDatabase.GetMongoDBConnected();

            AccountCollection = databaseConnected.GetCollection<Account>(AccountDocumentName);
        }

        public Account GetAccountByUsername(string username) {
            if (username == null) throw new ArgumentNullException("username");

            try {
                FilterDefinition<Account> usernameFilter =
                    Builders<Account>.Filter.Where(account => account.Username == username);

                FilterDefinition<Account> usernameAndPasswordFilter =
                    Builders<Account>.Filter.And(usernameFilter);

                return AccountCollection.Find(usernameAndPasswordFilter).FirstOrDefault();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }

        public Account CreateNewAccount(Account newAccount) {

            if (newAccount == null) throw new ArgumentNullException("newAccount");
            try {
                FilterDefinition<Account> usernameFilter =
                   Builders<Account>.Filter.Where(account => account.Username == newAccount.Username);

                Account savedAccount = AccountCollection.Find(usernameFilter).FirstOrDefault();

                if (savedAccount != null) 
                    throw new MongoClientException("Username: " + newAccount.Username + " existed");
                
                AccountCollection.InsertOne(newAccount);

                return newAccount;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                throw;
            }
        }

        public bool DeleteAccount(ObjectId accountId) {
            if (accountId == null) throw new ArgumentNullException("accountId");

            try {
                FilterDefinition<Account> accountIdFilter = Builders<Account>.Filter.Eq("_id", accountId);
                AccountCollection.DeleteOne(accountIdFilter);
                return true;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return false;
            }
        }

        public bool AddNewFriendForAccount(ObjectId accountId, ObjectId friendId) {
            throw new NotImplementedException();
        }

        public bool RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId) {
            throw new NotImplementedException();
        }
    }
}
