using Challenge.Api.Services;
using Challenge.Infrastructure.Models;
using MassTransit;

namespace Challenge.Api.ServiceBus;

public class MessageConsumer :
    IConsumer<ChatBotMessage>
{
    private readonly IChatService _chatService;

    public MessageConsumer(IChatService chatService)
    {
        _chatService = chatService;
    }

    public Task Consume(ConsumeContext<ChatBotMessage> context)
    {
        Console.WriteLine($"MessageReceived:{context.Message.Message} from user:{context.Message.BotName}");
        _chatService.ReceiveMessage(context.Message.BotName, context.Message.Message);

        return Task.CompletedTask;
    }
}

public class MessageConsumerDefinition :
    ConsumerDefinition<MessageConsumer>
{
    public MessageConsumerDefinition()
    {
        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MessageConsumer> consumerConfigurator)
    {

    }
}