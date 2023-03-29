using Firebase.Auth;
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.DataTranfers;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Web;

namespace SocialNetworkBE.Repositorys {
    public class RoomChatRepository {
        private IMongoCollection<RoomChat> RoomChatCollection { get; set; }
        private IMongoDatabase DatabaseConnected { get; set; }
        private const string RoomChatDocumentName = "RoomChat";

        public RoomChatRepository() {
            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            DatabaseConnected = MongoDatabase.GetMongoDBConnected();
            RoomChatCollection = DatabaseConnected.GetCollection<RoomChat>(RoomChatDocumentName);
        }

        public RoomChat GetRoomChatByPaticipate(List<ObjectId> paticipateIds) {
            try {
                var pipeline = new BsonDocument[]
                {
                new BsonDocument("$match",
                new BsonDocument("Participates",
                new BsonDocument("$in",
                new BsonArray { paticipateIds[0] }))),
                new BsonDocument("$match",
                new BsonDocument("Participates",
                new BsonDocument("$in",
                new BsonArray{ paticipateIds[1] })))};

                var pipelineDefinition = PipelineDefinition<RoomChat, RoomChat>.Create(pipeline);
                return RoomChatCollection.Aggregate(pipelineDefinition).FirstOrDefault();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }

        public RoomChat CreateRoomChat(RoomChat roomChat) {
            try {
                RoomChatCollection.InsertOne(roomChat);
                return roomChat;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }
    }
}