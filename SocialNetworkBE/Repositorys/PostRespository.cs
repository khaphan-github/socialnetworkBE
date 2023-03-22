using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetworkBE.Repository.Config;
using ServiceStack;
using System.IO.Packaging;
using System.Drawing.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;

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

        public bool DetetePostById(ObjectId postObjectId, ObjectId ownerId) {
            try {
                FilterDefinition<Post> postNeedDeleteFilter =
                    Builders<Post>.Filter.Eq("_id", postObjectId) &
                    Builders<Post>.Filter.Eq("OwnerId", ownerId);

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


        public async Task UpdateNumOfCommentOfPost(ObjectId postObjectId, int increaseValue) {
            try {
                var filter = Builders<Post>.Filter.Eq("_id", postObjectId);

                UpdateDefinition<Post> update = 
                    Builders<Post>.Update.Inc(post => post.NumOfComment, increaseValue);

                var options = new FindOneAndUpdateOptions<Post>() { 
                    ReturnDocument = ReturnDocument.After 
                };

                await PostCollection.FindOneAndUpdateAsync(filter, update, options);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
            }
        }

        public Like MakeALikeOfPost(ObjectId postObjectId, Like userLike) {
            throw new NotImplementedException();
        }

        public bool RemoveAlikeOfPost(ObjectId postObjectId, Guid LikeGuid) {
            throw new NotImplementedException();
        }
    }
}