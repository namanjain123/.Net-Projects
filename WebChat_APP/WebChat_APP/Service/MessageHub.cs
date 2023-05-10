using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace WebChat_APP.Service
{
    
    public class MessageHub : Hub
    {
        public async Task SendMessage(string sender,string message)
        {
            await Clients.All.SendAsync("Message Sent",sender, message);
        }
        
    }
}
