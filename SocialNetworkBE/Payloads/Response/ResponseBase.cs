using System;
using System.Text;

namespace SocialNetworkBE.Payload.Response {
    public class ResponseBase {
        public Guid Id { get;  } = Guid.NewGuid();
        public DateTime Timestamp { get; } = DateTime.Now;
        public string ApiVersion { get;  } = "v0.0.1";
        public Status Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ResponseBase() {  }
        public ResponseBase(Status status, string message, object data) {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}