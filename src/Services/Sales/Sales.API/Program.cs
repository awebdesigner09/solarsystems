using Microsoft.Extensions.Configuration;
using Sales.API;
using Sales.Application;
using Sales.Application.Data;
using Sales.Infrastructure;
using Sales.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddScoped<IQuoteRequestCounter, CachedQuoteRequestCounter>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.Decorate<IQuoteRepository, CachedQuoteRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

if(app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}

app.Run();
 