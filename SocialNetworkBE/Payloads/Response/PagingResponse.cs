
using System.Collections.Generic;

namespace SocialNetworkBE.Payloads.Response {
    public class PagingResponse {

        public int NumberOfElement { get; set; } = 0;
        public object Paging { get; set; }
        public string NextPageURL { get; set; }
        public string PreviousPageURL { get; set; }

        public PagingResponse(string endpoint,int page, int size, object paging) {
            string pagingEndpoint = endpoint + "page=";

            string pagingSize = "&size=" + size.ToString();

            string nextPageURL = pagingEndpoint + (page + 1).ToString() + pagingSize;
            string previousPageURL = pagingEndpoint;

            if (page == 0)
                previousPageURL += page.ToString() + pagingSize;
            else
                previousPageURL += (page - 1).ToString() + pagingSize;

            NumberOfElement = size;
            Paging = paging;
            NextPageURL = nextPageURL;
            PreviousPageURL = previousPageURL;
        }
    }
}