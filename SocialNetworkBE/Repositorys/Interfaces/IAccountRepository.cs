using MongoDB.Bson;
using SocialNetworkBE.Repositorys.DataModels;
using System.Threading.Tasks;

namespace SocialNetworkBE.Repositorys.Interfaces {
    public interface IAccountRepository {
        Account GetAccountByUsername(string username);
        Account CreateNewAccount(Account newAccount);
        bool DeleteAccount(ObjectId accountId);
        Task AddNewFriendForAccount(ObjectId accountId, ObjectId friendId);
        Task RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId);

    }
}