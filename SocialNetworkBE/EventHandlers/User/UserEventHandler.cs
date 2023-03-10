using LiteDB;
using ServiceStack;
using ServiceStack.DataAnnotations;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.Interfaces;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ObjectId = MongoDB.Bson.ObjectId;

namespace SocialNetworkBE.EventHandlers.User
{
    
    public class UserEventHandler
    {
        private readonly AccountResponsitory accountResponsitory = new AccountResponsitory();

        public ResponseBase AddNewFriendByAccount(ObjectId accountId, ObjectId friendId)
        {
            Task task1 = accountResponsitory.AddNewFriendForAccount(accountId, friendId);
            Task task2 = accountResponsitory.UpdateFriendWhen_AddNewFriendForAccount(accountId, friendId);
            var tasks = new Task[] {
                task1,
                task2
            };
            Task.WaitAll(tasks);
            Task.WhenAll(tasks).Wait();
            if(task1.IsCompleted && task2.IsCompleted)
            {
                return new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Add success",
                    Data = accountResponsitory.GetAccountByObjectId(accountId),
                };
            }
            return new ResponseBase()
            {
                Status = Status.Failure,
                Message = "Add failure",
                Data = accountResponsitory.GetAccountByObjectId(accountId),
            };

        }
        public ResponseBase RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId)
        {
            Task task1 = accountResponsitory.RemoveAFriendFromAccount(accountId, friendId);
            Task task2 = accountResponsitory.UpdateFriendWhen_RemoveAFriendFromAccount(accountId, friendId);
            var tasks = new Task[] {
                task1,
                task2
            };
            Task.WaitAll(tasks);
            Task.WhenAll(tasks).Wait();
            if (task1.IsCompleted && task2.IsCompleted)
            {
                return new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Remove success",
                    Data = accountResponsitory.GetAccountByObjectId(accountId),
                };
            }
            return new ResponseBase()
            {
                Status = Status.Failure,
                Message = "Remove failure",
                Data = accountResponsitory.GetAccountByObjectId(accountId),
            };

        }
    }
}