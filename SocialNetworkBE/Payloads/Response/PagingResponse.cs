
namespace SocialNetworkBE.Payloads.Response {
    public class PagingResponse {
        public object Paging { get; set; }
        public string NextPageURL { get; set; }
        public string PreviousPageURL { get; set; }
    }
}