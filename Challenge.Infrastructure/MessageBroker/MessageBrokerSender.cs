using MassTransit;

namespace Challenge.Infrastructure;

public interface IMessageBrokerSender
{
    Task SendToTopic<T>(T message) where T : class;
    Task SendToQueue<T>(T message, string queueName) where T : class;
}

public class MessageBrokerSender : IMessageBrokerSender
{
    private readonly IBus _eventBus;

    public MessageBrokerSender(IBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task SendToTopic<T>(T message) where T : class
    {
        var sendEndpoint = await _eventBus.GetPublishSendEndpoint<T>();
        await sendEndpoint.Send(message);
    }
    
    public async Task SendToQueue<T>(T message, string queueName) where T : class
    {
        var queueAddress = new Uri($"queue:{queueName}");
        var sendEvent = await _eventBus.GetSendEndpoint(queueAddress);
        await sendEvent.Send(message);
    }
}