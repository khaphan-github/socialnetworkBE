using LiteDB;
using MongoDB.Bson;
using MongoDB.Driver;
using ServiceStack;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.Interfaces;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BsonDocument = MongoDB.Bson.BsonDocument;
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

        public bool CheckUsernameExist(string Username)
        {
            FilterDefinition<Account> usernameFilter = Builders<Account>.Filter.Where(account => account.Username == Username);
            Account accountCheck = AccountCollection.Find(usernameFilter).FirstOrDefault();
            bool exist = (accountCheck != null);
            return (exist);
        }

        public bool CheckEmailExist(string Email)
        {
            FilterDefinition<Account> emailFilter = Builders<Account>.Filter.Where(account => account.Email == Email);
            Account accountCheck = AccountCollection.Find(emailFilter).FirstOrDefault();
            bool exist = (accountCheck != null);
            return (exist);
        }

        public bool ValidatePassword(string password, out string ErrorMessage)
        {
            var input = password;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new Exception("Password should not be empty");
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one lower case letter";
                return false;
            }
            else if (!hasUpperChar.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one upper case letter";
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(input))
            {
                ErrorMessage = "Password should not be less than or greater than 12 characters";
                return false;
            }
            else if (!hasNumber.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one numeric value";
                return false;
            }

            else if (!hasSymbols.IsMatch(input))
            {
                ErrorMessage = "Password should contain At least one special case characters";
                return false;
            }
            else
            {
                return true;
            }
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

        public string GetUserProfileUrlById(ObjectId id)
        {
            if (id == null) throw new ArgumentNullException("username");

            try
            {
                FilterDefinition<Account> idFilter =
                    Builders<Account>.Filter.Where(account => account.Id == id);


                return AccountCollection.Find(idFilter).FirstOrDefault().UserProfileUrl;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }
        public AccountResponseForGet GetAccountByObjectId(ObjectId id )
        {
            if (id == null) throw new ArgumentNullException("username");

            try
            {
                FilterDefinition<Account> idFilter =
                    Builders<Account>.Filter.Where(account => account.Id == id);
                Account accountFind = AccountCollection.Find(idFilter).FirstOrDefault();
                AccountResponseForGet accountGet = new AccountResponseForGet();
                accountGet.Id = id;
                accountGet.DisplayName = accountFind.DisplayName;
                accountGet.Email = accountFind.Email;
                accountGet.Username = accountFind.Username;
                accountGet.AvatarUrl = accountFind.AvatarUrl;
                accountGet.UserProfileUrl = accountFind.UserProfileUrl;
                accountGet.NumberOfFriend = accountFind.NumberOfFriend;
                accountGet.ListFriendsObjectId = accountFind.ListFriendsObjectId;
                accountGet.ListObjectId_GiveUserInvitation = accountFind.ListObjectId_GiveUserInvitation;
                accountGet.ListObjectId_UserSendInvite = accountFind.ListObjectId_UserSendInvite;
                accountGet.ListPostsObjectId = accountFind.ListPostsObjectId;
                return accountGet;
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


        public Account DeniedInvatationToOtherUser(ObjectId uid, ObjectId fid)
        {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", uid);
            Account accountUpdate = AccountCollection.Find(accountFilter).FirstOrDefault();
            bool friendExistInListInvite = accountUpdate.ListObjectId_GiveUserInvitation.Any(x => x == fid);
            if (friendExistInListInvite)
            {
                accountUpdate.ListObjectId_GiveUserInvitation.Remove(fid);
            }
            AccountCollection.ReplaceOne(b => b.Id == uid, accountUpdate);
            return accountUpdate;
        }
        public Account UpdateFriend_DeniedInvatationToOtherUser(ObjectId uid, ObjectId fid)
        {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", fid);
            Account accountUpdate = AccountCollection.Find(accountFilter).FirstOrDefault();
            bool friendExistInListInvite = accountUpdate.ListObjectId_UserSendInvite.Any(x => x == uid);
            if (friendExistInListInvite)
            {
                accountUpdate.ListObjectId_UserSendInvite.Remove(uid);
            }
            AccountCollection.ReplaceOne(b => b.Id == fid, accountUpdate);
            return accountUpdate;
        }

        public Account UpdateListInvitationOfFriendId_SendInvitationToOtherUser(ObjectId uid, ObjectId fid)
        {
            try
            {
                FilterDefinition<Account> accountIdFilter = Builders<Account>.Filter.Eq("_id", fid);
                Account accountUpdate = AccountCollection.Find(accountIdFilter).FirstOrDefault();


                if (accountUpdate.ListObjectId_GiveUserInvitation == null)
                {
                    accountUpdate.ListObjectId_GiveUserInvitation = new System.Collections.Generic.List<ObjectId>();
                    accountUpdate.ListObjectId_GiveUserInvitation.Add(uid);
                }
                else
                {
                    bool friendExist = accountUpdate.ListObjectId_GiveUserInvitation.Any(x => x == uid);
                    if (friendExist)
                    {
                        return null;
                    }
                    else
                    {
                        accountUpdate.ListObjectId_GiveUserInvitation.Add(uid);
                    }
                }
                AccountCollection.ReplaceOne(b => b.Id == fid, accountUpdate);
                return accountUpdate;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }

        public bool beFriend(ObjectId uid, ObjectId fid)
        {
            FilterDefinition<Account> accountIdFilter = Builders<Account>.Filter.Eq("_id", uid);
            Account accountUpdate = AccountCollection.Find(accountIdFilter).FirstOrDefault();
            bool friendExist = accountUpdate.ListFriendsObjectId.Any(x => x == fid);
            if (friendExist)
                return true;
            return false;
        }

        public (Account account, string result) SendInvitationToOtherUser(ObjectId uid, ObjectId fid)
        {
            try
            {
                FilterDefinition<Account> accountIdFilter = Builders<Account>.Filter.Eq("_id", uid);
                Account accountUpdate = AccountCollection.Find(accountIdFilter).FirstOrDefault();

                if (beFriend(uid, fid))
                    return (accountUpdate, "2 users are friend");
                
                if (accountUpdate.ListObjectId_UserSendInvite == null)
                {
                    accountUpdate.ListObjectId_UserSendInvite = new System.Collections.Generic.List<ObjectId>();
                    accountUpdate.ListObjectId_UserSendInvite.Add(fid);
                }
                else
                {
                    bool friendExist = accountUpdate.ListObjectId_UserSendInvite.Any(x => x == fid);
                    if (friendExist)
                    {
                        return (accountUpdate, "User send before");
                    }
                    else
                    {
                        accountUpdate.ListObjectId_UserSendInvite.Add(fid);
                    }
                }
                AccountCollection.ReplaceOne(b => b.Id == uid, accountUpdate);
                return (accountUpdate, "ok");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return (null,"error");
            }
        }


        public Account UpdateFriendWhen_RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId)
        {
            FilterDefinition<Account> friendFilter = Builders<Account>.Filter.Eq("_id", friendId);
            Account friendDeleteFriend = AccountCollection.Find(friendFilter).FirstOrDefault();
            if (friendDeleteFriend == null)
            {
                return null;
            }
            else
            {
                bool friendExist = friendDeleteFriend.ListFriendsObjectId.Any(x => x == accountId);
                if (!friendExist)
                {
                    return null;
                }
                else
                {
                    friendDeleteFriend.ListFriendsObjectId.Remove(accountId);
                    friendDeleteFriend.NumberOfFriend = friendDeleteFriend.ListFriendsObjectId.Count();
                }
            }
            AccountCollection.ReplaceOne(b => b.Id == friendId, friendDeleteFriend);
            return friendDeleteFriend;
        }
        public Account RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId) {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", accountId);
            Account accountDeleteFriend = AccountCollection.Find(accountFilter).FirstOrDefault();
            if(accountDeleteFriend == null)
            {
                return null;
            }
            else
            {
                bool friendExist = accountDeleteFriend.ListFriendsObjectId.Any(x => x == friendId);
                if (!friendExist)
                {
                    return null;
                }
                else
                {
                    accountDeleteFriend.ListFriendsObjectId.Remove(friendId);
                    accountDeleteFriend.NumberOfFriend = accountDeleteFriend.ListFriendsObjectId.Count();
                }
            }
            AccountCollection.ReplaceOne(b => b.Id == accountId, accountDeleteFriend);
            return accountDeleteFriend;
        }

        public List<FriendRespone> GetFriendsOfUserByUserId(ObjectId uid, int page, int size)
        {
            int paging = size * page;

            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", uid);
            List<ObjectId> accountGet = AccountCollection.Find(accountFilter).FirstOrDefault().ListFriendsObjectId;
            if (accountGet == null)
            {
                return null;
            }

            List<FriendRespone> friends = new List<FriendRespone>();
            foreach (ObjectId item in accountGet)
            {
                FriendRespone accountGetFriend = new FriendRespone();
                FilterDefinition<Account> accountFriendFilter = Builders<Account>.Filter.Eq("_id", item);
                Account accountFriendGet = AccountCollection.Find(accountFilter).FirstOrDefault();
                accountGetFriend.Id = item;
                accountGetFriend.DisplayName = accountFriendGet.DisplayName;
                accountGetFriend.Avatar = accountFriendGet.AvatarUrl;
                accountGetFriend.ProfileUrl = accountFriendGet.UserProfileUrl;
                friends.Add(accountGetFriend);
            }
           
            return friends;
        }

        public Account AcceptInvitationFromOtherUser(ObjectId uid, ObjectId fid)
        {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", uid);
            Account accountUpdate = AccountCollection.Find(accountFilter).FirstOrDefault();
            bool friendExistInListInvite = accountUpdate.ListObjectId_GiveUserInvitation.Any(x => x == fid);
            if(friendExistInListInvite)
            {
                if (accountUpdate.ListFriendsObjectId == null)
                {
                    accountUpdate.ListObjectId_GiveUserInvitation.Remove(fid);
                    accountUpdate.ListFriendsObjectId.Add(fid);
                    accountUpdate.NumberOfFriend = accountUpdate.ListFriendsObjectId.Count;
                }
                else
                {
                    accountUpdate.ListObjectId_GiveUserInvitation.Remove(fid);
                    accountUpdate.ListFriendsObjectId.Add(fid);
                    accountUpdate.NumberOfFriend = accountUpdate.ListFriendsObjectId.Count;
                }
            }
            AccountCollection.ReplaceOne(b => b.Id == uid, accountUpdate);
            return accountUpdate;
        }
        public Account UpdateFriend_AcceptInvitationFromOtherUser(ObjectId uid, ObjectId fid)
        {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", fid);
            Account accountUpdate = AccountCollection.Find(accountFilter).FirstOrDefault();
            bool friendExistInListInvite = accountUpdate.ListObjectId_UserSendInvite.Any(x => x == uid);
            if (friendExistInListInvite)
            {
                if (accountUpdate.ListFriendsObjectId == null)
                {
                    accountUpdate.ListObjectId_UserSendInvite.Remove(uid);
                    accountUpdate.ListFriendsObjectId.Add(uid);
                    accountUpdate.NumberOfFriend = accountUpdate.ListFriendsObjectId.Count;
                }
                else
                {
                    accountUpdate.ListObjectId_UserSendInvite.Remove(uid);
                    accountUpdate.ListFriendsObjectId.Add(uid);
                    accountUpdate.NumberOfFriend = accountUpdate.ListFriendsObjectId.Count;
                }
            }
            AccountCollection.ReplaceOne(b => b.Id == fid, accountUpdate);
            return accountUpdate;
        }

        public Account UpdateAccount(AccountRequest accountRequest, ObjectId accId)
        {
            FilterDefinition<Account> accountFilter = Builders<Account>.Filter.Eq("_id", accId);
            Account accountUpdate = AccountCollection.Find(accountFilter).FirstOrDefault();
            accountUpdate.DisplayName = accountRequest.DisplayName;
            accountUpdate.AvatarUrl = accountRequest.AvatarUrl;
            accountUpdate.Username = accountRequest.Username;
            BCryptService bCryptService = new BCryptService();
            string password = accountUpdate.Password = accountRequest.Password;
            string randomSalt = bCryptService.GetRandomSalt();
            string secretKey = ServerEnvironment.GetServerSecretKey();
            string passwordHash =
                bCryptService
                .HashStringBySHA512(bCryptService.GetHashCode(randomSalt, password, secretKey));
            accountUpdate.Email = accountRequest.Email;
            accountUpdate.HashSalt = passwordHash;
            AccountCollection.ReplaceOne(b => b.Id == accId, accountUpdate);
            return accountUpdate;
        }

        public async Task<List<BsonDocument>> GetListAccountsMetadata(List<ObjectId> accounts, int page, int size) {
            try {

                var filter = Builders<Account>.Filter.In(x => x.Id, accounts);
                var projection = Builders<Account>.Projection
                    .Include(account => account.DisplayName)
                    .Include(account => account.UserProfileUrl)
                    .Include(account => account.AvatarUrl)
                    .Include(account => account.Id);

                return await AccountCollection.Find(filter).Project(projection).Skip(page * size).Limit(size).ToListAsync();

            } catch (Exception) {
                return new List<BsonDocument>();
            }
        }
    }
}
