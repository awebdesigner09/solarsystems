using BuildingBlocks.Messaging.MassTransit;
using Sales.Application;
using Sales.Application.Common.Interfaces;
using Sales.Infrastructure;
using Sales.Infrastructure.Realtime;
using Sales.QuotesWorker;
using Sales.QuotesWorker.EventHandlers.Integration;
using System.Reflection;

var builder = Host.CreateApplicationBuilder(args);

// Add services
builder.Services
    .AddAppServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

var app = builder.Build();
app.Run();
