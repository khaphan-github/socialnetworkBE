
using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace SocialNetworkBE.Repositorys {

    public class CommentRepository {
        private IMongoCollection<Comment> CommentCollection { get; set; }
        private IMongoDatabase databaseConnected { get; set; }
        private const string PostDocumentName = "Comment";

        public CommentRepository() {
            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            databaseConnected = MongoDatabase.GetMongoDBConnected();
            CommentCollection = databaseConnected.GetCollection<Comment>(PostDocumentName);
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

        public UpdateResult UpdateCommentByComentId(ObjectId commentid, string content) {
            try {
                FilterDefinition<Comment> commentFilter =
                    Builders<Comment>.Filter.Eq("_id", commentid);

                UpdateDefinition<Comment> contentUpdate = Builders<Comment>.Update.Set("Content", content);

                return CommentCollection.UpdateOne(commentFilter, contentUpdate);

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public DeleteResult DeteteCommentById(ObjectId commentid) {
            try {
                FilterDefinition<Comment> commentFilter =
                    Builders<Comment>.Filter.Eq("_id", commentid);

                return CommentCollection.DeleteOne(commentFilter);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }
    }
}