﻿using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetworkBE.Repositorys.DataModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SocialNetworkBE.Repository
{
    public class AccountResponsitory
    {

        MongoClient Client = new MongoClient("mongodb+srv://kimkhanhneee0201hihi:KimKhanh0201@cluster0.oc1roht.mongodb.net/?retryWrites=true");

        public Account GetAccByUsernamePwd(string username, string pwd)
        {
            var DB = Client.GetDatabase("SocialNetwork");

            var collection = DB.GetCollection<BsonDocument>("Account");

            var allAccs = collection.Find(new BsonDocument()).ToList();
            
            Account acc = allAccs
                .Select(account => new SocialNetworkBE.Repositorys.DataModels.Account
                {
                    Id = account["_id"].AsObjectId,
                    Username = account["username"].AsString,
                    Email = account["email"].AsString,
                    Password = account["pwd"].AsString,

                    UserProfileUrl = account["userProfileUrl"].AsString,
                    CreatedAt = (DateTime)account["create_at"].AsBsonDateTime,
                    DisplayName = account["displayname"].AsString,

                    AvatarUrl = account["avturl"].AsString,
                    NumofFriend = account["numOfFriends"].AsInt32,
                    ListFriend = account["listFriends"].AsBsonArray.ToArray(),
                    ListPost = account["listPosts"].AsBsonArray.ToArray(),

                }).FirstOrDefault(x=> x.Username == username && x.Password == pwd);
            return acc;
        }
    }
}
