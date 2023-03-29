using Microsoft.VisualStudio.TestTools.UnitTesting;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Services.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing.UnitTesting {
    [TestClass]
    public class HttpClientServiceTest {

        private PushNotificationService _pushNotificationService;
        [TestMethod]
        public async Task SendAccessTokenToNotifiService_ReturnsTrue() {
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

        public async Task PushMessage_ReturnsTrue() {

            Notify notify = new Notify() {
                CreatedDate= DateTime.Now,
                Message = "Test message from .net",
            };
             
            _pushNotificationService = new PushNotificationService("1233", notify);

            bool result = await _pushNotificationService.PushMessage();
            // Assert
            Assert.IsTrue(result);
        }
    }
}
