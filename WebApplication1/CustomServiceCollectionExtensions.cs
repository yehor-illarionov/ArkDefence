using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public static class CustomServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMasstransit(this IServiceCollection services)
        {
            // local function to create the bus
            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                return Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    cfg.ReceiveEndpoint("tenant-created", ep =>
                    {
                         //ep.
                    });
                });
            }

            // local function to configure consumers
            void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            {
               // configurator.AddConsumer<OrderConsumer>();
            }

            // configures MassTransit to integrate with the built-in dependency injection
            services.AddMassTransit(CreateBus, ConfigureMassTransit);
            return services;
        }
    }
}
