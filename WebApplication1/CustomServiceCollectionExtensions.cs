using MassTransit;
using MassTransit.AspNetCoreIntegration;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebApplication1.Consumers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using GreenPipes;

namespace WebApplication1
{
    /// <summary>
    /// кастомная конфигурация и импортируемые (внешние) сервисы
    /// </summary>
    public static class CustomServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomMasstransit(this IServiceCollection services)
        {
            // local function to create the bus
            IBusControl CreateBus(IServiceProvider serviceProvider)
            {
                return Bus.Factory.CreateUsingInMemory(cfg =>
                {
                    //cfg.ReceiveEndpoint("tenant-created", ep =>
                    //{
                    //    ep.UseMessageRetry(r => r.Interval(2, 100));
                    //    ep.ConfigureConsumer<TenantCreatedConsumer>(serviceProvider);
                    //});
                    cfg.ReceiveEndpoint("tenant-created-notdefault", ep =>
                    {
                        ep.UseMessageRetry(r => r.Interval(2, 100));
                        ep.ConfigureConsumer<TenantCreatedNotDefaultConsumer>(serviceProvider);
                    });
                    cfg.UseInMemoryScheduler();
                });
            }

            // local function to configure consumers
            void ConfigureMassTransit(IServiceCollectionConfigurator configurator)
            {
               // configurator.AddConsumer<TenantCreatedConsumer>();
                configurator.AddConsumer<TenantCreatedNotDefaultConsumer>();
            }

            // configures MassTransit to integrate with the built-in dependency injection
            services.AddMassTransit(CreateBus, ConfigureMassTransit);
            return services;
        }
    }
}
