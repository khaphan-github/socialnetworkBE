using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SocialNetworkBE.Repository
{
    public class AccountResponsitory
    {

        MongoClient Client = new MongoClient("mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true");

        public IEnumerable<Account> GetAccByUsernamePwd(string username, string pwd)
        {
            var DB = Client.GetDatabase("SocialNetwork");

            var collection = DB.GetCollection<BsonDocument>("Account");

            var allAccs = collection.Find(new BsonDocument()).ToList();
            
            IEnumerable<SocialNetworkBE.Repositorys.DataModels.Account> accs = allAccs
                .Select(account => new SocialNetworkBE.Repositorys.DataModels.Account
                {
                    Id = account["_id"].AsObjectId,
                    Username = account["username"].AsString,
                    Email = account["email"].AsString,
                    Password = account["pwd"].AsString,
                    CreatedAt = account["create_at"].AsDateTime,
                    UpdatedAt = account["update_at"].AsDateTime,
                    AvatarUrl = account["avturl"].AsString
                });
            IEnumerable<SocialNetworkBE.Repositorys.DataModels.Account> accResults = accs.Where(x => x.Username == username && x.Password == pwd);
            return accResults;
        }
    }
}
