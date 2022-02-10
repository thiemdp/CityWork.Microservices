using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using GreenPipes;
using System.Reflection;

namespace CityWork.Infrastructure
{
    public static class MessageBrokerExtensions
    {
        public static IServiceCollection AddMessageBrokerWithMassTransit(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddScoped<IPublishEvent, PublishEvent>();
            if (appSettings.MessageBroker.UsedMessageBroker())
            {
                if (appSettings.MessageBroker.UsedRabbitMQ)
                {
                    services.AddMassTransit(configure =>
                    {
                        configure.AddConsumers(Assembly.GetEntryAssembly());

                        configure.UsingRabbitMq((context, configurator) =>
                        {
                            configurator.Host(appSettings.MessageBroker.RabbitMQ.HostName, h =>
                            {
                                h.Username(appSettings.MessageBroker.RabbitMQ.UserName);
                                h.Password(appSettings.MessageBroker.RabbitMQ.Password);
                            });
                            configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter("CityWorkMessage", false));
                            configurator.UseMessageRetry(retryConfigurator =>
                            {
                                retryConfigurator.Intervals(
                                    TimeSpan.FromSeconds(5),
                                    TimeSpan.FromSeconds(5),
                                    TimeSpan.FromSeconds(5),
                                    TimeSpan.FromSeconds(10),
                                    TimeSpan.FromSeconds(15),
                                    TimeSpan.FromSeconds(30),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(60),
                                    TimeSpan.FromSeconds(3600),
                                    TimeSpan.FromSeconds(3600),
                                    TimeSpan.FromSeconds(3600),
                                    TimeSpan.FromSeconds(3600),
                                    TimeSpan.FromSeconds(3600)
                                    );
                                retryConfigurator.Ignore<ArgumentNullException>();
                            });
                        });
                    });
                    services.AddMassTransitHostedService();
                }
            }
            return services;
        }
    }
}
