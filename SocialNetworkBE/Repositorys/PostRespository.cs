using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetworkBE.Repository.Config;
using ServiceStack;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using Amazon.Runtime.Documents;
using System.Collections.ObjectModel;
using MongoDB.Driver.Linq;
using SocialNetworkBE.Payloads.Response;
using MongoDB.Bson.Serialization;

namespace SocialNetworkBE.Repository {
    public class PostRespository {
        private IMongoCollection<Post> PostCollection { get; set; }
        private IMongoDatabase DatabaseConnected { get; set; }
        private const string PostDocumentName = "Post";

        public PostRespository() {
            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            DatabaseConnected = MongoDatabase.GetMongoDBConnected();

            PostCollection = DatabaseConnected.GetCollection<Post>(PostDocumentName);
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

        public List<BsonDocument> GetPostByPageAndSizeAndSorted(int page, int size, string sortType) {
            try {
                int paging = page * size;
                var sort = sortType == "desc"
                    ? Builders<BsonDocument>.Sort.Descending("UpdateAt")
                    : Builders<BsonDocument>.Sort.Ascending("UpdateAt");

                IMongoCollection<BsonDocument> PostCollectionBsonDocument =
                    DatabaseConnected.GetCollection<BsonDocument>(PostDocumentName);

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
                    .Sort(sort)
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

                UpdateDefinition<Post> update = Builders<Post>.Update
                    .Inc(post => post.NumOfComment, increaseValue)
                    .Set(post => post.UpdateAt, DateTime.Now);

                var options = new FindOneAndUpdateOptions<Post>() {
                    ReturnDocument = ReturnDocument.After
                };

                await PostCollection.FindOneAndUpdateAsync(filter, update, options);

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
            }
        }
        public async Task<Post> UpdateAPost(ObjectId postObjectId, Post PostUpdate)
        {
            try
            {
                var filter = Builders<Post>.Filter.Eq("_id", postObjectId);

                UpdateDefinition<Post> update = Builders<Post>.Update
                    .Set(post => post, PostUpdate);

                var options = new FindOneAndUpdateOptions<Post>()
                {
                    ReturnDocument = ReturnDocument.After
                };

               await PostCollection.FindOneAndUpdateAsync(filter, update, options);
                return GetPostById(postObjectId);
               
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return null;
            }
        }

        public async Task MakeALikeOfPostAsync(ObjectId postObjectId, ObjectId userId) {
            try {
                var filter = Builders<Post>.Filter.Eq("_id", postObjectId);

                UpdateDefinition<Post> update = Builders<Post>.Update
                       .Set(post => post.UpdateAt, DateTime.Now)
                       .Push(post => post.Likes, userId)
                       .Inc(post => post.NumOfLike, 1);

                await PostCollection.UpdateOneAsync(filter, update);

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
            }
        }

        public async Task RemoveAlikeOfPostAsync(ObjectId postObjectId, ObjectId userId) {
            try {
                var filter = Builders<Post>.Filter.Eq(post => post.Id, postObjectId);
                var update = Builders<Post>.Update
                    .Pull(post => post.Likes, userId)
                    .Inc(post => post.NumOfLike, -1);

                await PostCollection.UpdateOneAsync(filter, update);
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
            }
        }

        public BsonDocument GetUserMetadataLikedPost(ObjectId postObjectId, int page, int size) {
            try {
                int paging = page * size;

                var filter = Builders<Post>.Filter.Eq(post => post.Id, postObjectId);
                var projection = Builders<Post>.Projection.Include(post => post.Likes);

                return PostCollection.Find(filter).Project(projection).Skip(paging).Limit(size).FirstOrDefault();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return new BsonDocument();
            }
        }
    }
}