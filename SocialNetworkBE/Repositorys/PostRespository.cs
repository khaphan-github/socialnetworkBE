using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using SocialNetworkBE.Repository.Config;
using SocialNetworkBE.Repositorys.Interfaces;

namespace SocialNetworkBE.Repository {
    public class PostRespository : IPostRepository {
        private IMongoCollection<Post> PostCollection { get; set; }

        public PostRespository() {
            const string PostDocumentName = "Post";

            MongoDBConfiguration MongoDatabase = new MongoDBConfiguration();
            IMongoDatabase databaseConnected = MongoDatabase.GetMongoDBConnected();

            PostCollection = databaseConnected.GetCollection<Post>(PostDocumentName);
        }

        public List<Post> GetPostByUserId(ObjectId userObjectId) {
            try {
                FilterDefinition<Post> userOwnedPostFilter = Builders<Post>.Filter.Eq("OwnerId", userObjectId);
            
                return PostCollection.Find(userOwnedPostFilter).ToList();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                throw;
            }
        }

        public Post GetPostById(ObjectId postObjectId) {
            try {
                FilterDefinition<Post> userOwnedPostFilter = Builders<Post>.Filter.Eq("_id", postObjectId);

                return PostCollection.Find(userOwnedPostFilter).FirstOrDefault();
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                throw;
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
                FilterDefinition<Post> postNeedDeleteFilter = Builders<Post>.Filter.Eq("_id", postObjectId);
                PostCollection.DeleteOne(postNeedDeleteFilter);

                return true;
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return false;
            }
        }

        public List<Post> GetPostByPageAndSizeAndSorted(int page, int size) {
            try {
                int paging = page * size;
                
                FilterDefinition < Post > justUpdatePostFilter = Builders<Post>.Filter.Empty;

                var topLevelProjectionResults = PostCollection
                    .Find(justUpdatePostFilter)
                    .SortByDescending(post => post.CreateDate)
                    .Skip(paging)
                    .Limit(size)
                    .ToList();

                return topLevelProjectionResults;

            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("[ERROR]: " + ex.Message);
                return new List<Post>();
            }
        }

        public bool DeteteCommentByGuid(ObjectId postObjectId, Guid CommentGuid) {
            throw new NotImplementedException();
        }

        public Comment MakeACommentToPost(ObjectId postObjectId, Comment comment) {
            throw new NotImplementedException();
        }

        public Comment UpdateAcommentByGuid(ObjectId postObjectId, Guid commentGuid) {
            throw new NotImplementedException();
        }

        public List<Comment> GetCommentsByPostIdWithPaging(ObjectId postObjectId, int page, int size) {
            throw new NotImplementedException();
        }

        public Like MakeALikeOfPost(ObjectId postObjectId, Like userLike) {
            throw new NotImplementedException();
        }

        public bool RemoveAlikeOfPost(ObjectId postObjectId, Guid LikeGuid) {
            throw new NotImplementedException();
        }
    }
}
