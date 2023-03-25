using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Repositorys {

    public class CommentRepository {
        private IMongoCollection<Comment> CommentCollection { get; set; }
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

        public List<Comment> GetCommentOfPostWithPaging(ObjectId postId, int page, int size) {
            try {
                int paging = page * size;

                FilterDefinition<Comment> userOwnedPostFilter =
                    Builders<Comment>.Filter.Eq("PostId", postId);

                List<Comment> commentOfPosts = CommentCollection
                    .Find(userOwnedPostFilter)
                    .Skip(paging)
                    .Limit(size)
                    .ToList();

                return commentOfPosts;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Comment> { null };
            }
        }

        public List<Comment> GetChildrenCommentsByParentId(ObjectId postId, ObjectId parentId, int page, int size) {
            try {
                int paging = page * size;

                FilterDefinition<Comment> childrenCommentFilter =
                    Builders<Comment>.Filter.Eq("PostId", postId) &
                    Builders<Comment>.Filter.Eq("ParentId", parentId);

                List<Comment> commentOfPosts = CommentCollection
                    .Find(childrenCommentFilter)
                    .Skip(paging)
                    .Limit(size)
                    .ToList();

                return commentOfPosts;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<Comment> { null };
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