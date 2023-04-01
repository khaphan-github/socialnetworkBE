using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Services.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.UnitTesting
{
    [TestClass]
    public class HttpClientServiceTest
    {

        private PushNotificationService _pushNotificationService;
        [TestMethod]
        public async Task SendAccessTokenToNotifiService_ReturnsTrue()
        {
            // Arrange
            string userId = "testUserId";
            string accessToken = "testAccessToken";
            _pushNotificationService = new PushNotificationService(userId, accessToken);
            // Act
            bool result = await _pushNotificationService.SendAccessTokenToNotifiService();

            // Assert
            Assert.IsTrue(result);
        }


        [TestMethod]

        public async Task PushMessageInvitation_ReturnsTrue()
        {
            string userId = "6423e4184c08244aca2724fb";
            string userReceive = "64229848df27353d469ff935";
            _pushNotificationService = new PushNotificationService(userId, "");
            bool result = await _pushNotificationService.PushMessage_WhenReceiveInvitation(userReceive);
            Assert.IsTrue(result);
        }


        [TestMethod]

        public async Task PushMessageComment_ReturnsTrue()
        {
            string userId = "6423e4184c08244aca2724fb";
            string userReceive = "64229848df27353d469ff935";
            string commentId = "640cafe67b17bd7ec5d21916";
            string postId = "64248fd5cd70ff58f1b05616";
            _pushNotificationService = new PushNotificationService(userId, "");
            bool result = await _pushNotificationService.PushMessage_Whencomment(userReceive, commentId, postId);
            Assert.IsTrue(result);
        }

        [TestMethod]

        public async Task PushMessageLike_ReturnsTrue()
        {
            string userId = "6423e4184c08244aca2724fb";
            string userReceive = "64229848df27353d469ff935";
            string postId = "64248fd5cd70ff58f1b05616";
            _pushNotificationService = new PushNotificationService(userId, "");
            bool result = await _pushNotificationService.PushMessage_WhenLike(userReceive, postId);
            Assert.IsTrue(result);
        }
    }
}