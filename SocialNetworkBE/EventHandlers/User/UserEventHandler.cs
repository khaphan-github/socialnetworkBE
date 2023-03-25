using LiteDB;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Web;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.Interfaces;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Firebase;
using SocialNetworkBE.Services.Hash;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ObjectId = MongoDB.Bson.ObjectId;

namespace SocialNetworkBE.EventHandlers.User
{
    
    public class UserEventHandler
    {
        private readonly AccountResponsitory accountResponsitory = new AccountResponsitory();

        public async Task<ResponseBase> UpdateAccount(AccountRequest account, ObjectId accId, HttpPostedFile Media)
        {
            FirebaseImage firebaseService = new FirebaseImage();
            List<string> mediaURLList = new List<string>();

            if (Media == null)
            {
                return new ResponseBase()
                {
                    Status = Status.WrongFormat,
                    Message = "File not allow null",
                };
            }

            if (Media != null)
            {
                    string mediaName = Guid.NewGuid().ToString() + ".png";

                    string folder = "AvatarUrl";

                    await firebaseService.UploadImageAsync(Media.InputStream, folder, mediaName);

                    string imageDownloadLink = firebaseService.StorageDomain + "/" + folder + "/" + mediaName;

                    account.AvatarUrl = imageDownloadLink;
            }
            try
            {
                return new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Update success",
                    Data = accountResponsitory.UpdateAccount(account, accId)
                };
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Error:" + ex);
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Update failure"
                };
            }
        }

        public ResponseBase RemoveAFriendFromAccount(ObjectId accountId, ObjectId friendId)
        {
            Account accountDelete = accountResponsitory.RemoveAFriendFromAccount(accountId, friendId);
            Account accountUpdateDelete = accountResponsitory.UpdateFriendWhen_RemoveAFriendFromAccount(accountId, friendId);

            if (accountDelete != null && accountUpdateDelete!= null)
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
            var resulString = accountResponsitory.SendInvitationToOtherUser(accountId, friendId);
            Account accountSent = accountResponsitory.SendInvitationToOtherUser(accountId, friendId).account;
            Account friendReceive = accountResponsitory.UpdateListInvitationOfFriendId_SendInvitationToOtherUser(accountId, friendId);
            if (resulString.result == "2 users are friend")
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "2 users are friend before",
                };
            }
            else if (resulString.result == "User send before")
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "User sent invitation before",
                };
            }
            else if (accountSent != null && friendReceive != null)
            {
                
                return new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Send success",
                    Data = accountResponsitory.GetAccountByObjectId(accountId) ,
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

        public ResponseBase AcceptInvitationFromOtherUser(ObjectId uid, ObjectId fid)
        {
            Account userAccept = accountResponsitory.AcceptInvitationFromOtherUser(uid, fid);
            Account friendAccept = accountResponsitory.UpdateFriend_AcceptInvitationFromOtherUser(uid, fid);
            if (userAccept!= null && friendAccept!= null)
            {
                FriendRespone friendRespone = new FriendRespone();
                friendRespone.Id = fid;
                friendRespone.DisplayName = friendAccept.DisplayName;
                friendRespone.Avatar = friendAccept.AvatarUrl;
                friendRespone.ProfileUrl = friendAccept.UserProfileUrl;
                return new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Add success",
                    Data = friendRespone,
                };
            }
            return new ResponseBase()
            {
                Status = Status.Failure,
                Message = "Add failure",
            };
        }

        public ResponseBase DeniedInvatationToOtherUser(ObjectId uid, ObjectId fid)
        {
            Account accountDenied = accountResponsitory.DeniedInvatationToOtherUser(uid, fid);
            Account accountUpdateDeined = accountResponsitory.UpdateFriend_DeniedInvatationToOtherUser(uid, fid);
            if (accountDenied != null && accountUpdateDeined != null)
            {
                return new ResponseBase()
                {
                    Status = Status.Success,
                    Message = "Deny success",
                    Data = accountResponsitory.GetAccountByObjectId(uid),
                };
            }
            return new ResponseBase()
            {
                Status = Status.Failure,
                Message = "Deny failure",
            };
        }
    }
}