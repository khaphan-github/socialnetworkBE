using MongoDB.Bson;
using System.Collections.Generic;
using System;
using SocialNetworkBE.Repositorys.DataModels;

namespace SocialNetworkBE.Payloads.Response {
    public class PostResponse {
        public ObjectId Id { get; set; }
        public ObjectId OwnerId { get; set; }
        public string OwnerAvatarURL { get; set; }
        public string OwnerDisplayName { get; set; }
        public string OwnerProfileURL { get; set; }
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public string Scope { get; set; } = "public";
        public string Content { get; set; }
        public List<string> Media { get; set; }
        public int NumOfComment { get; set; } = 0;
        public string CommentsURL { get; set; } 
        public int NumOfLike { get; set; } = 0;
        public string LikesURL { get; set;}

        public static PostResponse ConvertPostToPostResponse(Post post) {
            return new PostResponse {
                Id = post.Id, 
                OwnerId = post.OwnerId,
                OwnerAvatarURL = post.OwnerAvatarURL,
                OwnerDisplayName = post.OwnerDisplayName,
                OwnerProfileURL = post.OwnerProfileURL,
                NumOfLike = post.NumOfLike,
                CommentsURL = post.CommentsURL,
                Content= post.Content,
                Media = post.Media,
                NumOfComment= post.NumOfComment,
                Scope= post.Scope,
                UpdateAt = post.UpdateAt,
                LikesURL= post.LikesURL,
            };
        } 
    }
}