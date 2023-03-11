using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetworkBE.Repository.Config;
using MongoDB.Bson.Serialization;
using ServiceStack;

namespace SocialNetworkBE.Repository {
    public class PostRespository {
        private IMongoCollection<Post> PostCollection { get; set; }
        private IMongoDatabase databaseConnected { get; set; }
        private const string PostDocumentName = "Post";

        public PostRespository() {
            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            databaseConnected = MongoDatabase.GetMongoDBConnected();

            PostCollection = databaseConnected.GetCollection<Post>(PostDocumentName);
        }

        public List<Post> GetPostByUserId(ObjectId userObjectId) {
            try {
                FilterDefinition<Post> userOwnedPostFilter = Builders<Post>.Filter.Eq("OwnerId", userObjectId);

                return PostCollection.Find(userOwnedPostFilter).ToList();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return new List<Post> { null };
            }
        }

        public Post GetPostById(ObjectId postObjectId) {
            try {
                FilterDefinition<Post> userOwnedPostFilter = Builders<Post>.Filter.Eq("_id", postObjectId);

                return PostCollection.Find(userOwnedPostFilter).FirstOrDefault();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }

        public Post CreateNewPost(Post NewPost) {
            try {
                PostCollection.InsertOne(NewPost);
                return NewPost;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return null;
            }
        }

        public bool DetetePostById(ObjectId postObjectId) {
            try {
                FilterDefinition<Post> postNeedDeleteFilter =
                    Builders<Post>.Filter.Eq("_id", postObjectId);

                Post deletePost = PostCollection.FindOneAndDelete(postNeedDeleteFilter);
                if (deletePost != null) 
                    return true;

                return false;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return false;
            }
        }

        public List<BsonDocument> GetPostByPageAndSizeAndSorted(int page, int size) {
            try {
                int paging = page * size;

                IMongoCollection<BsonDocument> PostCollectionBsonDocument =
                    databaseConnected.GetCollection<BsonDocument>(PostDocumentName);

                FilterDefinition<BsonDocument> justUpdatePostFilter = Builders<BsonDocument>.Filter.Empty;

                var projection = Builders<BsonDocument>.Projection
                    .Include("OwnerId")
                    .Include("OwnerAvatarURL")
                    .Include("OwnerDisplayName")
                    .Include("OwnerProfileURL")
                    .Include("UpdateAt")
                    .Include("Scope")
                    .Include("Content")
                    .Include("Media")
                    .Include("NumOfComment")
                    .Include("CommentsURL")
                    .Include("NumOfLike")
                    .Include("LikesURL");

                var topLevelProjectionResults = PostCollectionBsonDocument
                    .Find(justUpdatePostFilter)
                    .Project(projection)
                    .Skip(paging)
                    .Limit(size)
                    .ToList();

                return topLevelProjectionResults;

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return new List<BsonDocument>();
            }
        }

        public bool DeteteCommentOfPostByGuid(ObjectId postObjectId, Guid CommentGuid) {
            throw new NotImplementedException();
        }

        public Comment MakeACommentToPost(ObjectId postObjectId, Comment comment) {
            throw new NotImplementedException();
        }

        public Comment UpdateAcommentByGuid(ObjectId postObjectId, Guid commentGuid) {
            throw new NotImplementedException();
        }

        public List<Comment> GetCommentsByPostIdWithPaging(ObjectId postObjectId, int page, int size) {
            try {

                int paging = page * size;

                IMongoCollection<BsonDocument> postCollectionBsonDocument =
                    databaseConnected.GetCollection<BsonDocument>(PostDocumentName);

                FilterDefinition<BsonDocument> postFilter =
                    Builders<BsonDocument>.Filter.Eq("_id", postObjectId);

                // TODO: Need to optimize performance query comment - not get all comment then paging

                var commentProject =
                    Builders<BsonDocument>.Projection.Slice("Comments", paging, size);

                var commentOfPost = postCollectionBsonDocument
                    .Find(postFilter)
                    .Project(commentProject)
                    .FirstOrDefault();

                if (commentOfPost == null) {
                    return new List<Comment>();
                }

                Post savedPost = BsonSerializer.Deserialize<Post>(commentOfPost);


            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
            }

            return new List<Comment>();
        }

        public Like MakeALikeOfPost(ObjectId postObjectId, Like userLike) {
            throw new NotImplementedException();
        }

        public bool RemoveAlikeOfPost(ObjectId postObjectId, Guid LikeGuid) {
            throw new NotImplementedException();
        }
    }
}