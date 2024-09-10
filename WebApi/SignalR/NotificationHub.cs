using Microsoft.AspNetCore.SignalR;

namespace WebApi.SignalR
{
    public class NotificationHub: Hub
    {
        public async Task SendMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
