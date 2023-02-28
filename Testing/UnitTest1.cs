using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repository;
using System.Linq;

namespace Testing
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Getaccs()
        {
            AccountResponsitory account = new AccountResponsitory();
            IEnumerable<Account> accresult = account.GetAccByUsernamePwd("kimkhanh21", "0201hihi");
            int isValidAccount = accresult.Count();
            Assert.IsTrue(isValidAccount != 0);
        }
    }
}
