using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Sales.API;
using Sales.API.Hubs;
using Sales.API.Services;
using Sales.Application;
using Sales.Application.Data;
using Sales.Domain.Identity;
using Sales.Infrastructure;
using Sales.Infrastructure.Data;
using Sales.Infrastructure.Data.Extensions;
using System.Text;

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
// 1. Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 2. Add Authentication and JWT Bearer
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false; // In production, this should be true
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
        };
    });

// 3. Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(myAppCorsPolicy);
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();
}
app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationsHub>("/notificationsHub");
app.Run();
 