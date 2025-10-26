using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
   public static class Extensions
    {
        public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();
                foreach(var asm in assemblies.Where(a => a is not null))
                    config.AddConsumers(asm);

                config.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration["MessageBroker:Host"], host =>
                    {
                        host.Username(configuration["MessageBroker:Username"]!);
                        host.Password(configuration["MessageBroker:Password"]!);
                    });
                    configurator.ConfigureEndpoints(context);
                });
            });
            return services;
        }
    }
}
