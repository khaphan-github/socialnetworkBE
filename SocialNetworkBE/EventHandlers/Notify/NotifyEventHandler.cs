using SocialNetworkBE.Services.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SocialNetworkBE.EventHandlers {
    public class NotifyEventHandler {
        // **
        // - Nếu thống báo là dạng kết bạn - thì chỉ tạo 1 thông báo cho thằng được gửi
        // - Nếu thông báo là bài post mới - thì tạo thông báo cho top 10 - 20 bạn của thằng tạo post
        // - Nếu thông báo là comment bài post - thì tạo thông báo cho thằng chủ post 
        // - Nếu thông báo là comment 1 comment - thì tạo thông báo cho thằng chủ comment và chủ post
        // */

        /** Flow thông báo:
           1/  Khi gửi lời mời kết bạn - Lưu thông vô db - Signalr Ping client theo uid - nếu online - gửi thông báo - nếu offline thì thôi
           2/  
        */
        private readonly SignalRService SignalRService;

        public NotifyEventHandler() {
            SignalRService = new SignalRService();
        }

        public async Task SendNotify(string connectionId, object message) {
            if(await SignalRService.IsClientOnline(connectionId)) {
                await SignalRService.SendNotificationTo(connectionId, message);
            }
        }
    }
}