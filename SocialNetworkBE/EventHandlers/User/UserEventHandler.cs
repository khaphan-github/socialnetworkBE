using LiteDB;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Web;
using SocialNetworkBE.Payload.Response;
using SocialNetworkBE.Payloads.Data;
using SocialNetworkBE.Payloads.Request;
using SocialNetworkBE.Payloads.Response;
using SocialNetworkBE.Repository;
using SocialNetworkBE.Repositorys.DataModels;
using SocialNetworkBE.Repositorys.Interfaces;
using SocialNetworkBE.ServerConfiguration;
using SocialNetworkBE.Services.Firebase;
using SocialNetworkBE.Services.Hash;
using SocialNetworkBE.Services.Notification;
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
        private PushNotificationService _pushNotificationService;

        public async Task<ResponseBase> UpdateAccount(AccountRequest account, ObjectId accId, HttpPostedFile Media)
        {
            FirebaseImage firebaseService = new FirebaseImage();
            Debug.WriteLine(Media);
            if (Media != null && Media.ContentLength > 0)
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

        public async Task<ResponseBase> SendInvitationToOtherUser(ObjectId accountId, ObjectId friendId)
        {
            _pushNotificationService =new PushNotificationService(accountId.ToString(), "");
            await _pushNotificationService.PushMessage_WhenReceiveInvitation(friendId.ToString());
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
            AccountResponseForGet userGet = accountResponsitory.GetAccountByObjectId(uid);
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
                Data = userGet,
            };
        }

        public ResponseBase GetUserProfileUrlById(ObjectId uid)
        {
            string userUrlGet = accountResponsitory.GetUserProfileUrlById(uid);
            if (userUrlGet == null)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "Get url failure"
                };
            }
            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Get url success",
                Data = "User's profile: " + userUrlGet,
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

        public ResponseBase GetFriendOfUserByUserId(ObjectId userId, int page, int size)
        {

            List<FriendRespone> friendRespones = accountResponsitory.GetFriendsOfUserByUserId(userId, page, size);

            if (friendRespones.Count == 0)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "There are not friend of this user",
                };
            }

            string pagingEndpoint = "/api/v1/user/friends?";
            PagingResponse pagingResponse =
                new PagingResponse(pagingEndpoint, page, friendRespones.Count, friendRespones);

            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Get list friend of this friend success",
                Data = pagingResponse
            };
        }

        public ResponseBase GetAllInvitationById(ObjectId userId, int page, int size)
        {

            List<AccountRespone> accountRespones = accountResponsitory.getAllinvitation(userId);

            if (accountRespones.Count == 0)
            {
                return new ResponseBase()
                {
                    Status = Status.Failure,
                    Message = "There are not invitations of this user",
                };
            }

            string pagingEndpoint = "/api/v1/user/invitations?";
            PagingResponse pagingResponse =
                new PagingResponse(pagingEndpoint, page, accountRespones.Count, accountRespones);

            return new ResponseBase()
            {
                Status = Status.Success,
                Message = "Get all invitations of this friend success",
                Data = pagingResponse
            };
        }
    }
}