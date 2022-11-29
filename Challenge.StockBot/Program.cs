using Challenge.Infrastructure;
using MassTransit;
using Challenge.StockBot;
using Challenge.StockBot.MessageBroker;

var builder = WebApplication.CreateBuilder(args);

var rabbitMqUri = builder.Configuration["RabbitMqUri"] ??
                  throw new ArgumentNullException("Configuration[\"RabbitMqUri\"]");
var rabbitMqVirtualHost = builder.Configuration["RabbitMqVirtualHost"] ??
                          throw new ArgumentNullException("Configuration[\"RabbitMqVirtualHost\"]");
var rabbitMqUser = builder.Configuration["RabbitMqUser"] ??
                   throw new ArgumentNullException("Configuration[\"RabbitMqUser\"]");
var rabbitMqPassword = builder.Configuration["RabbitMqPassword"] ??
                       throw new ArgumentNullException("Configuration[\"RabbitMqPassword\"]");

builder.Services.AddTransient<IStooqServiceApi, StooqServiceApi>();
builder.Services.AddTransient<IStockService, StockBotService>();
builder.Services.AddTransient<IMessageBrokerSender,MessageBrokerSender>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MessageBrokerConsumer>(typeof(MessageBrokerConsumerDefinition));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqUri, rabbitMqVirtualHost, h =>
        {
            h.Username(rabbitMqUser);
            h.Password(rabbitMqPassword);
        });
        cfg.ReceiveEndpoint("stockbot.queue", e =>
        {
            e.ConfigureConsumer<MessageBrokerConsumer>(context);
            e.Bind("chat.commandRequest", x =>
            {
                x.Durable = true;
                x.ExchangeType = "fanout";
                x.AutoDelete = false;
            });
        });
    });
});

var app = builder.Build();

app.Run();