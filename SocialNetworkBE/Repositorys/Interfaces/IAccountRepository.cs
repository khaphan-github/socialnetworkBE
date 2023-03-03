using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkBE.Repositorys.Interfaces {
    public interface IAccountRepository {
        Account GetAccountByUsernameAndPassword(string username, string password);
    }
}
