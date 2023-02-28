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
            Account accresult = account.GetAccByUsernamePwd("kimkhanh21", "0201hihi");
            string isValid = accresult.Username;
            Assert.IsTrue(isValid.Equals("kimkhanh21"));
        }
    }
}
