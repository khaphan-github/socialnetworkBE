
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkBE.Services.Http {
    public class HttpClientService {
        private readonly HttpClient httpClient;
        private readonly string contentType = "application/json";
        public HttpClientService() {
            httpClient = new HttpClient();
        }
        public async Task<HttpResponseMessage> PostAsync(string url, string content) {
            var httpContent = new StringContent(content, Encoding.UTF8, contentType);
            return await httpClient.PostAsync(url, httpContent);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, string content) {
            var httpContent = new StringContent(content, Encoding.UTF8, contentType);
            return await httpClient.PutAsync(url, httpContent);
        }
    }
}