using Sales.Application;
using Sales.Infrastructure;
using Sales.QuotesWorker.EventHandlers.Integration;
using MassTransit;
var builder = Host.CreateApplicationBuilder(args);

// Add services
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

//// Configure MassTransit
//builder.Services.AddMassTransit(config =>
//{
//    // Add the consumer
//    config.AddConsumer<QuoteRequestEventHandler>();

//    config.UsingRabbitMq((context, cfg) =>
//    {
//        cfg.Host(builder.Configuration["MessageBroker:Host"], "/", h =>
//        {
//            h.Username(builder.Configuration["MessageBroker:Username"]);
//            h.Password(builder.Configuration["MessageBroker:Password"]);
//        });

//        // Configure the endpoint for QuoteRequestEvent
//        cfg.ReceiveEndpoint("quote-requests-queue", e =>
//        {
//            e.ConfigureConsumer<QuoteRequestEventHandler>(context);
//        });
//    });
//});

// Remove the default worker as we're using MassTransit consumer
// builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
