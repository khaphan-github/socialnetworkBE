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
                Message = "Remove failure"
            };

        }

        public ResponseBase SendInvitationToOtherUser(ObjectId accountId, ObjectId friendId)
        {
            Task task1 = accountResponsitory.SendInvitationToOtherUser(accountId, friendId);
            Task task2 = accountResponsitory.UpdateListInvitationOfFriendId_SendInvitationToOtherUser(accountId, friendId);
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
                    Message = "Send success",
                    Data = accountResponsitory.GetAccountByObjectId(accountId),
                };
            }
            return new ResponseBase()
            {
                Status = Status.Failure,
                Message = "Send failure"
            };
        }

        public ResponseBase GetUserProfileById(ObjectId uid)
        {
            var userGet = accountResponsitory.GetAccountByObjectId(uid);
            if(userGet == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Get user failure"
                };
            }
            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Get user success",
                Data = accountResponsitory.GetAccountByObjectId(uid),
            };
        }

        public ResponseBase GetFriendOfUserByUserId(ObjectId uid)
        {
            var listFriendGet = accountResponsitory.GetFriendsOfUserByUserId(uid,1 ,1);
            if (listFriendGet == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Get user's friends failure"
                };
            }
            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Get user's friends success",
                Data = listFriendGet,
            };
        }
    }
}