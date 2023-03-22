
namespace SocialNetworkBE.Payloads.Response {
    public class PagingResponse {

        public int NumberOfElement { get; set; } = 0;
        public object Paging { get; set; }
        public string NextPageURL { get; set; }
        public string PreviousPageURL { get; set; }
    }
}