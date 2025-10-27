using Sales.Application;
using Sales.Infrastructure;
using Sales.QuotesWorker.EventHandlers.Integration;
using Sales.QuotesWorker;
using BuildingBlocks.Messaging.MassTransit;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

// Add services
builder.Services
    .AddAppServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);


var host = builder.Build();
host.Run();
