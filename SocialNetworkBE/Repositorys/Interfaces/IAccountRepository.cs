using MongoDB.Bson;
using SocialNetworkBE.Repositorys.DataModels;

namespace SocialNetworkBE.Repositorys.Interfaces {
    public interface IAccountRepository {
        Account GetAccountByUsernameAndPassword(string username, string password);
        Account CreateNewAccount(Account newAccount);
        bool DeleteAccount(ObjectId accountId);
        bool AddNewFriendForAccount(ObjectId accountId, ObjectId friendId);
        bool RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId);

    }
}