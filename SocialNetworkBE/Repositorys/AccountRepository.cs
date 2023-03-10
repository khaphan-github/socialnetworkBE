using LiteDB;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using ObjectId = MongoDB.Bson.ObjectId;

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
        public Account GetAccountByObjectId(ObjectId id )
        {
            if (id == null) throw new ArgumentNullException("username");

            try
            {
                FilterDefinition<Account> idFilter =
                    Builders<Account>.Filter.Where(account => account.Id == id);


                return AccountCollection.Find(idFilter).FirstOrDefault();
            }
            catch (Exception ex)
            {
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


        public async Task UpdateFriendWhen_AddNewFriendForAccount(ObjectId accountId, ObjectId friendId)
        {
            FilterDefinition<Account> friendIdFilter = Builders<Account>.Filter.Eq("_id", friendId);
            Account friendUpdate = AccountCollection.Find(friendIdFilter).FirstOrDefault();

            if (friendUpdate.ListFriendsObjectId == null)
            {
                friendUpdate.ListFriendsObjectId = new System.Collections.Generic.List<ObjectId>();
                friendUpdate.ListFriendsObjectId.Add(accountId);
                friendUpdate.NumberOfFriend = friendUpdate.ListFriendsObjectId.Count();
            }
            else
            {
                bool accountExist = friendUpdate.ListFriendsObjectId.Any(x => x == accountId);
                if(accountExist)
                {
                    return;
                }
                else
                {
                    friendUpdate.ListFriendsObjectId.Add(accountId);
                    friendUpdate.NumberOfFriend = friendUpdate.ListFriendsObjectId.Count();
                }
            }
            
            await AccountCollection.ReplaceOneAsync(b => b.Id == friendId, friendUpdate);
        }

        public async Task  AddNewFriendForAccount(ObjectId accountId, ObjectId friendId) {

            try
            {
                FilterDefinition<Account> accountIdFilter = Builders<Account>.Filter.Eq("_id", accountId);
                Account accountUpdate = AccountCollection.Find(accountIdFilter).FirstOrDefault();
                

                if (accountUpdate.ListFriendsObjectId == null)
                {
                    accountUpdate.ListFriendsObjectId = new System.Collections.Generic.List<ObjectId> ();
                    accountUpdate.ListFriendsObjectId.Add(friendId);
                    accountUpdate.NumberOfFriend = accountUpdate.ListFriendsObjectId.Count();
                }
                else
                {
                    bool friendExist = accountUpdate.ListFriendsObjectId.Any(x => x == friendId);
                    if(friendExist)
                    {
                        return;
                    }
                    else
                    {
                        accountUpdate.ListFriendsObjectId.Add(friendId);
                        accountUpdate.NumberOfFriend = accountUpdate.ListFriendsObjectId.Count();
                    }
                }
                await AccountCollection.ReplaceOneAsync(b => b.Id == accountId, accountUpdate);
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
            }
        }


        public async Task UpdateFriendWhen_RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId)
        {
            FilterDefinition<Account> friendFilter = Builders<Account>.Filter.Eq("_id", accountId);
            Account friendDeleteFriend = AccountCollection.Find(friendFilter).FirstOrDefault();
            if (friendDeleteFriend == null)
            {
                return;
            }
            else
            {
                bool friendExist = friendDeleteFriend.ListFriendsObjectId.Any(x => x == accountId);
                if (friendExist)
                {
                    return;
                }
                else
                {
                    friendDeleteFriend.ListFriendsObjectId.Remove(accountId);
                    friendDeleteFriend.NumberOfFriend = friendDeleteFriend.ListFriendsObjectId.Count();
                }
            }
            await AccountCollection.ReplaceOneAsync(b => b.Id == friendId, friendDeleteFriend);
        }
        public async Task RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId) {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", accountId);
            Account accountDeleteFriend = AccountCollection.Find(accountFilter).FirstOrDefault();
            if(accountDeleteFriend == null)
            {
                return;
            }
            else
            {
                bool friendExist = accountDeleteFriend.ListFriendsObjectId.Any(x => x == friendId);
                Debug.WriteLine(friendExist);
                if (friendExist)
                {
                    return;
                }
                else
                {
                    accountDeleteFriend.ListFriendsObjectId.Remove(friendId);
                    accountDeleteFriend.NumberOfFriend = accountDeleteFriend.ListFriendsObjectId.Count();
                }
            }
            await AccountCollection.ReplaceOneAsync(b => b.Id == accountId, accountDeleteFriend);
        }
    }
}
