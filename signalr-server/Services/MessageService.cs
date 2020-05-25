using Microsoft.AspNetCore.SignalR;
using signalr.server.Hubs;
using System;
using System.Threading.Tasks;

namespace signalr.server.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<ChatHub> _hubContext;
        public MessageService(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task SendAllAsync(int index)
        {
            await _hubContext.Clients.All.SendAsync("NewMessage", 
                                                        "Server", 
                                                        $"New message {index}");
        }
    }
}
