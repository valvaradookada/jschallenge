using Challenge.Api.Domain;
using Challenge.Api.Hubs;
using Challenge.Infrastructure;
using Challenge.Infrastructure.Data;
using Challenge.Infrastructure.Data.Models;
using Challenge.Infrastructure.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Challenge.Api.Services;

public interface IChatService
{
    Task ReceiveMessage(string user, string message);
    Task<List<ChatMessage>> GetPreviousMessages();
}

public class ChatService : IChatService
{
    private IHubContext<ChatHub, IChatClient> _hubContext { get; }
    private readonly IMessageBrokerSender _messageBrokerSender;
    private readonly IChatCommand _chatCommand;
    private readonly ChatDbContext _chatDbContext;

    public ChatService(IHubContext<ChatHub, IChatClient> chatHubContext,
        IMessageBrokerSender messageBrokerSender, IChatCommand chatCommand, ChatDbContext chatDbContext)
    {
        _hubContext = chatHubContext;
        _messageBrokerSender = messageBrokerSender;
        _chatCommand = chatCommand;
        _chatDbContext = chatDbContext;
    }

    public async Task ReceiveMessage(string user, string message)
    {
        _chatCommand.Message = message;
        if (_chatCommand.ValidCommand)
        {
            await _messageBrokerSender.SendToTopic(new ChatCommandRequest
            {
                Command = _chatCommand.Command,
                BotName = _chatCommand.BotName
            });
        }
        else
        {
            //if not a command, store in db
            await _chatDbContext.ChatMessages.AddAsync(new ChatMessage() {Message = message, User = user});
            await _chatDbContext.SaveChangesAsync();
        }
        await _hubContext.Clients.All.ReceiveMessage(user, message);
    }

    public async Task<List<ChatMessage>> GetPreviousMessages()
    {
        //Todo: Max amount of messages should be read from environment variables
        var result = await _chatDbContext.ChatMessages.AsNoTracking()
            .OrderByDescending(message => message.ReceivedDateTime).Take(50)
            .OrderBy(message => message.ReceivedDateTime).ToListAsync();
        return result;
    }
}