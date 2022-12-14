using Microsoft.AspNetCore.SignalR;

namespace Challenge.Api.Hubs
{
    public interface IChatClient
    {
        Task ReceiveMessage(string user, string message);
    }
    public class ChatHub : Hub<IChatClient>
    {
        public async Task SendMessage(string user, string message)
            => await Clients.All.ReceiveMessage(user, message);
    }
}