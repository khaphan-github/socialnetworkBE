using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.DataTranfers;
using SocialNetworkBE.Services.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace SocialNetworkBE.Repositorys {

    public class CommentRepository {
        private IMongoCollection<Comment> CommentCollection { get; set; }
        private PushNotificationService _pushNotificationService;

        private IMongoDatabase DatabaseConnected { get; set; }
        private const string PostDocumentName = "Comment";

        public CommentRepository() {
            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            DatabaseConnected = MongoDatabase.GetMongoDBConnected();
            CommentCollection = DatabaseConnected.GetCollection<Comment>(PostDocumentName);
        }

        public Comment CreateCommentAPost(Comment comment) {
            try {
                CommentCollection.InsertOne(comment);
                return comment;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public Task<CommentDataTranfer> GetCommentById(ObjectId commentId) {
            try {
                var queryDocumentPipeline = PipelineDefinition<Comment, CommentDataTranfer>.Create(
                    new BsonDocument("$match",
                    new BsonDocument("_id", commentId)),
                    new BsonDocument("$lookup",
                    new BsonDocument
                    {
                    { "from", "Account" },
                    { "localField", "OwnerId" },
                    { "foreignField", "_id" },
                    { "as", "Account_Mapping" }
                    }),
                    new BsonDocument("$set",
                    new BsonDocument("Account_Mapping",
                    new BsonDocument("$first", "$Account_Mapping"))),
                    new BsonDocument("$set",
                    new BsonDocument
                    {
                    { "OwnerDisplayName", "$Account_Mapping.DisplayName" },
                    { "OwnerAvatarURL", "$Account_Mapping.AvatarUrl" },
                    { "OwnerProfileURL", "$Account_Mapping.DisplayName" }
                    }),
                    new BsonDocument("$unset", "Account_Mapping"));

                return CommentCollection.Aggregate(queryDocumentPipeline).FirstOrDefaultAsync();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Task.FromResult<CommentDataTranfer>(null);
            }
        }

        public Task<List<CommentDataTranfer>> GetCommentOfPostWithPaging(ObjectId postId, ObjectId? parrentCommentId, int page, int size) {
            try {
                int paging = page * size;

                var queryDocumentPipeline = PipelineDefinition<Comment, CommentDataTranfer>.Create(
                    new BsonDocument("$match", new BsonDocument {{ "PostId", postId },{ "ParentId", parrentCommentId } }),

                    new BsonDocument("$skip", paging),
                    new BsonDocument("$limit", size),

                    new BsonDocument("$lookup", new BsonDocument
                    {
                        { "from", "Account" },
                        { "localField", "OwnerId" },
                        { "foreignField", "_id" },
                        { "as", "AccountMapping" }
                    }),

                    new BsonDocument("$set", new BsonDocument("AccountMapping", new BsonDocument("$first", "$AccountMapping"))),
                    new BsonDocument("$set", new BsonDocument
                    {
                        { "OwnerDisplayName", "$AccountMapping.DisplayName" },
                        { "OwnerAvatarURL", "$AccountMapping.AvatarUrl" },
                        { "OwnerProfileURL", "$AccountMapping.UserProfileUrl" }
                    }),

                    new BsonDocument("$unset", "AccountMapping")
                );

                return CommentCollection.Aggregate(queryDocumentPipeline).ToListAsync();

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Task.FromResult<List<CommentDataTranfer>>(null);
            }
        }



        // @param count: 1 is increase += 1, -1 to descrease -=1
        public Task<UpdateResult> UpdateNumberOfChildrent(ObjectId commentId, int count) {
            try {
                FilterDefinition<Comment> commentFilter =
                    Builders<Comment>.Filter.Eq("_id", commentId);

                UpdateDefinition<Comment> contentUpdate = 
                    Builders<Comment>.Update.Inc("CommentCount", count);

                return CommentCollection.UpdateOneAsync(commentFilter, contentUpdate);

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return Task.FromResult<UpdateResult>(null);
            }
        }
        public UpdateResult UpdateCommentByComentId(ObjectId commentid, ObjectId ownerId, string content) {
            try {
                FilterDefinition<Comment> commentFilter =
                    Builders<Comment>.Filter.Eq("_id", commentid) &
                     Builders<Comment>.Filter.Eq("OwnerId", ownerId);

                UpdateDefinition<Comment> contentUpdate = Builders<Comment>.Update.Set("Content", content);

                return CommentCollection.UpdateOne(commentFilter, contentUpdate);

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public DeleteResult DeteteCommentById(ObjectId commentid, ObjectId ownerId) {
            try {
                FilterDefinition<Comment> commentFilter =
                    Builders<Comment>.Filter.Eq("_id", commentid) &
                    Builders<Comment>.Filter.Eq("OwnerId", ownerId);

                return CommentCollection.DeleteOne(commentFilter);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}