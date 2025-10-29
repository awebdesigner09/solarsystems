using Sales.API;
using Sales.API.Hubs;
using Sales.API.Services;
using Sales.Application;
using Sales.Application.Data;
using Sales.Infrastructure;
using Sales.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);
// Define CORS policy
var myAppCorsPolicy = "myAppCorsPolicy";

builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            name: myAppCorsPolicy,
            policy =>
            {
                policy.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
            });
    });

// Add services to the container.
builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

builder.Services.AddSignalR();
builder.Services.AddScoped<IQuoteRequestCounter, CachedQuoteRequestCounter>();
builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
builder.Services.Decorate<IQuoteRepository, CachedQuoteRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddHostedService<RedisSubscriberService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();
app.UseCors(myAppCorsPolicy);

if(app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
app.MapHub<NotificationsHub>("/notificationsHub");
app.Run();
 