using Microsoft.AspNetCore.SignalR;
using signalr.server.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace signalr.server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ICounterService _counterService;
        public ChatHub(ICounterService counterService)
        {
            _counterService = counterService;
        }

        public async Task SendMessage(string connectionid, string message)
        {
            _counterService.MessagesReplied.Add(new KeyValuePair<string, string>(connectionid, message));
            Console.WriteLine($"Received a new message => Total messages received: {_counterService.MessagesReplied.Count}");
        }        

        public async override Task OnConnectedAsync()
        {
            _counterService.ConnectionIds.TryAdd(Context.ConnectionId, Context.ConnectionId);
            Console.WriteLine($"New client connected: {Context.ConnectionId} => Total connections: {_counterService.ConnectionIds.Count}");
            await Clients.Client(Context.ConnectionId).SendAsync("Connected", Context.ConnectionId, _counterService.ConnectionIds.Count);
        }    

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _counterService.ConnectionIds.TryRemove(Context.ConnectionId, out string value);
            Console.WriteLine($"Client disconnected: {Context.ConnectionId} => Total connections: {_counterService.ConnectionIds.Count}");
            return base.OnDisconnectedAsync(exception);
        }
    }
}
