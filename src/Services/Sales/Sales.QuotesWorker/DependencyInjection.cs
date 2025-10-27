using BuildingBlocks.Behaviors;
using BuildingBlocks.Messaging.MassTransit;
using Sales.Application.Data;
using System.Reflection;

namespace Sales.QuotesWorker
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppServices
            (this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
            });
            services.AddMessageBroker(configuration, Assembly.GetExecutingAssembly());
            services.AddScoped<IQuoteRepository, QuoteRepository>();
            services.Decorate<IQuoteRepository, CachedQuoteRepository>();
            return services;
        }
    }
}
