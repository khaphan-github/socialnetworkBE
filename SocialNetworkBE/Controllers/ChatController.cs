using MongoDB.Bson;
using SocialNetworkBE.EventHandlers.Chat;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using System.Web.Http;

namespace SocialNetworkBE.Controllers {
    [RoutePrefix("api/v1/chat")]
    public class ChatController : ApiController {

        private readonly ChatEventHandler ChatEventHandler = new ChatEventHandler();
        [HttpPost]
        [Route("")]
        public ResponseBase GetChatRoomByPaticipateId([FromBody] RoomPaticipate roomPaticipate) {
            ObjectId.TryParse(roomPaticipate.FirstId, out ObjectId firstId);
            ObjectId.TryParse(roomPaticipate.SecondId, out ObjectId secondId);

            return ChatEventHandler.GetRoomChat(firstId, secondId);
        }
    }
}