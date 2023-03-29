using SocialNetworkBE.Services.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using static ServiceStack.Diagnostics.Events;

namespace SocialNetworkBE.Services.Notification {
    public class PushNotificationService
    {
        public string UserId { get; set; }
        public object Message { get; set; } // Object then stringtify

        public readonly string NotifiServiceEndpoint = "http://localhost:3005/api/v1/service/notify";
        public HttpClientService HttpService { get; set; }
        public PushNotificationService(string userId, object message)
        {
            UserId = userId;
            Message = message;
            HttpService = new HttpClientService();
        }
        

        public async Task<bool> PushMessage()
        {
            string contentFormat = "{{\"UserId\":\"{0}\",\"Message\":{1}}}";
            string messageJson = JsonSerializer.Serialize<object>(Message);

            string postContent = string.Format(contentFormat, new object[2] { UserId, messageJson });

            HttpResponseMessage httpResponse =
                await HttpService.PostAsync(NotifiServiceEndpoint, postContent);
            return httpResponse.IsSuccessStatusCode;
        }


        public async Task<bool> SendAccessTokenToNotifiService()
        {
            string contentFormat = "{{\"UserId\":\"{0}\",\"AccessToken\":\"{1}\"}}";
            string putContent = string.Format(contentFormat, new object[2] { UserId, Message });

            HttpResponseMessage httpResponse =
                await HttpService.PutAsync(NotifiServiceEndpoint, putContent);
            return httpResponse.IsSuccessStatusCode;
        }
    }
}