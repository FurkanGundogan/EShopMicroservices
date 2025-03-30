using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;

public static class Extentions
{

    public static IServiceCollection AddMessageBroker
        (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        // implement rabbitmq conf
        services.AddMassTransit(config =>
        {

            config.SetKebabCaseEndpointNameFormatter();

            if (assembly != null) config.AddConsumers(assembly);
            // important for consuming from basket and ordering 
            // for publisher it is null

            config.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:UserName"]);
                    host.Password(configuration["MessageBroker:Password"]);
                });
                configurator.ConfigureEndpoints(context);
                // auto configuration endpoints for consumers
            });


        });
        return services;
    }

}
