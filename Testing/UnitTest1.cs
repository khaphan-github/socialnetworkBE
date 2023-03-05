using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.Controllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repository;
using System.Linq;
using Newtonsoft.Json.Linq;
using SocialNetworkBE.Services.Firebase;
using Amazon.Runtime.Internal;
using System.Drawing;

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

        [TestMethod]

        public void PostImg()
        {

            FirebaseImage.UploadImage(@"C:/anh/kk.jpg").Wait();
            Assert.IsTrue(1 > 0);
        }

        [TestMethod]
        public void GetImg()
        {
            FirebaseImage.getUrl().Wait();
            Assert.IsTrue(1 > 0);
        }

    }
}
