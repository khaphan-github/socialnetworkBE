using Microsoft.AspNet.SignalR;
using System;

namespace SocialNetworkBE.Services.SignalR {
    public class SignalRService :Hub {
        public void Ping() {
            Clients.Client(Context.ConnectionId).Pong("SIGNALR  Connection Establish!");
        }
        private void Log(string message) {
            string time = DateTime.Now.ToString();
            System.Diagnostics.Debug.WriteLine(time + ": " + message);
        }
    }
}