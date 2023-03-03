using MongoDB.Bson;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;

namespace SocialNetworkBE.Repositorys.Interfaces {
    public interface IPostRepository {
        List<Post> GetPostByUserId(ObjectId userObjectId);
        Post GetPostById(ObjectId postObjectId);
        Post CreateNewPost(Post NewPost);
        bool DetetePostById(ObjectId postObjectId);

        bool DeteteCommentOfPostByGuid(ObjectId postObjectId, Guid CommentGuid);
        Comment MakeACommentToPost(ObjectId postObjectId, Comment comment);
        Comment UpdateAcommentByGuid(ObjectId postObjectId, Guid commentGuid);
        List<Comment> GetCommentsByPostIdWithPaging(ObjectId postObjectId, int page, int size);
        List<Post> GetPostByPageAndSizeAndSorted(int page, int size);

        Like MakeALikeOfPost(ObjectId postObjectId, Like userLike);
        bool RemoveAlikeOfPost(ObjectId postObjectId, Guid LikeGuid);        
    }
}
