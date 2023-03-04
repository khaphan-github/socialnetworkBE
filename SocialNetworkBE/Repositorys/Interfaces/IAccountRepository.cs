using MongoDB.Bson;
using SocialNetworkBE.Repositorys.DataModels;

namespace SocialNetworkBE.Repositorys.Interfaces {
    public interface IAccountRepository {
        Account GetAccountByUsername(string username);
        Account CreateNewAccount(Account newAccount);
        bool DeleteAccount(ObjectId accountId);
        bool AddNewFriendForAccount(ObjectId accountId, ObjectId friendId);
        bool RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId);

    }
}