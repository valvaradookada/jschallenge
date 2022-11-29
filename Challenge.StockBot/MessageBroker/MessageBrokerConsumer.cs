using Challenge.Infrastructure.Models;
using MassTransit;

namespace Challenge.StockBot.MessageBroker;
public class MessageBrokerConsumer :
    IConsumer<ChatCommandRequest>
{
    private readonly IStockService _stockService;

    public MessageBrokerConsumer(IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task Consume(ConsumeContext<ChatCommandRequest> context)
    {
        if (context.Message.BotName.ToLower() == "stock")
            await _stockService.GetStockQuote(context.Message.Command);
    }
}

public class MessageBrokerConsumerDefinition :
    ConsumerDefinition<MessageBrokerConsumer>
{
    public MessageBrokerConsumerDefinition()
    {
        ConcurrentMessageLimit = 1;
    }

    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<MessageBrokerConsumer> consumerConfigurator)
    {
    }
}