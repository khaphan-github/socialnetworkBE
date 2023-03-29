using MongoDB.Bson;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Repositorys.DataModels {
    public class RoomChat {
        public ObjectId Id { get; set; }
        [Unique]
        public List<ObjectId> Participates { get; set; }
        public Guid DocumentStored { get; set; }
        public DateTime CreatedAt { get; set; }

        public RoomChat(List<ObjectId> paticipates) {
            this.Id = new ObjectId();
            this.Participates = paticipates;
            this.DocumentStored = Guid.NewGuid();
            this.CreatedAt= DateTime.Now;
        }
    }
}