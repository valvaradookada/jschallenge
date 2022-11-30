using Challenge.Api.Domain;
using Challenge.Infrastructure;
using Challenge.Infrastructure.Models;
using Challenge.Api.Hubs;
using Challenge.Api.ServiceBus;
using Challenge.Api.Services;
using Challenge.Infrastructure.Data;
using MassTransit;


var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

var rabbitMqUri = builder.Configuration["RabbitMqUri"] ??
                  throw new ArgumentNullException("Configuration[\"RabbitMqUri\"]");
var rabbitMqVirtualHost = builder.Configuration["RabbitMqVirtualHost"] ??
                          throw new ArgumentNullException("Configuration[\"RabbitMqVirtualHost\"]");
var rabbitMqUser = builder.Configuration["RabbitMqUser"] ??
                   throw new ArgumentNullException("Configuration[\"RabbitMqUser\"]");
var rabbitMqPassword = builder.Configuration["RabbitMqPassword"] ??
                       throw new ArgumentNullException("Configuration[\"RabbitMqPassword\"]");

// Add services to the container.

var connectionString = configuration.GetConnectionString("chat") ?? "Data Source=chat.db";

builder.Services.AddSqlite<ChatDbContext>(connectionString);

builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddCors(o => o.AddPolicy("WebClientCorsPolicy", builder =>
{
    builder.SetIsOriginAllowed((host) => true)
        .WithOrigins("https://localhost:7180")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
}));
builder.Services.AddTransient<IChatService,ChatService>();
builder.Services.AddTransient<IChatCommand,ChatCommand>();
builder.Services.AddTransient<IMessageBrokerSender,MessageBrokerSender>();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<MessageConsumer>(typeof(MessageConsumerDefinition));
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqUri, rabbitMqVirtualHost, h =>
        {
            h.Username(rabbitMqUser);
            h.Password(rabbitMqPassword);
        });
        cfg.Message<ChatCommandRequest>(x => { x.SetEntityName("chat.commandRequest"); });
        cfg.Publish<ChatCommandRequest>(x =>
        {
            x.Durable = true;
            x.ExchangeType = "fanout";
            x.AutoDelete = false;
        });
        cfg.ReceiveEndpoint("chat.queue", e =>
        {
            e.ConfigureConsumer<MessageConsumer>(context);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ChatDbContext>();
    db.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapControllers();
app.UseRouting();
app.UseCors("WebClientCorsPolicy");
app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.Run();