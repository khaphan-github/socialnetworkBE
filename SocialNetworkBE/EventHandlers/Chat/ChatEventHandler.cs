using MongoDB.Bson;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Repositorys;
using SocialNetworkBE.Repositorys.DataModels;
using System.Collections.Generic;

namespace SocialNetworkBE.EventHandlers.Chat {
    public class ChatEventHandler {
        private readonly RoomChatRepository RoomChatRepository = new RoomChatRepository();

        public ResponseBase GetRoomChat(ObjectId firstParicipate, ObjectId secondParticipate) {
            List<ObjectId> paricipateId = new List<ObjectId>() { firstParicipate, secondParticipate };
            RoomChat roomChat = RoomChatRepository.GetRoomChatByPaticipate(paricipateId);

            if (roomChat == null) {
                roomChat = new RoomChat(paricipateId);
                RoomChatRepository.CreateRoomChat(roomChat);
            }

            return new ResponseBase() {
                Status = Status.Success,
                Message = "Get room chat success",
                Data = roomChat
            };

        }
    }
}