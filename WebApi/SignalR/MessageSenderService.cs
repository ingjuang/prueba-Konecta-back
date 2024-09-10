
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace WebApi.SignalR
{
    public class MessageSenderService : BackgroundService
    {

        private readonly IHubContext<NotificationHub> _hubContext;

        public MessageSenderService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var message = $"Mensaje enviado a las {DateTime.Now}";

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
