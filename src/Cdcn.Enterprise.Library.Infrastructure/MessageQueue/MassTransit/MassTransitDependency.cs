using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Cdcn.Enterprise.Library.Infrastructure.MessageQueue.MassTransit
{
    public static class MassTransitDependency
    {
        public static IServiceCollection AddRabbitMqMassTransit(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("MassTransit__ConnectionString");
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    //cfg.Host(connectionString);
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    // Specify the receive endpoint for the specific queue
                    //cfg.ReceiveEndpoint("specific-queue", e =>
                    //{
                    //    e.ConfigureConsumer<MyMessageConsumer>(context);
                    //});
                });
            });

            // services.AddMassTransitHostedService();


            return services;
        }
    }
}
