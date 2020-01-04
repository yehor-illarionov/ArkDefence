using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Consumers
{
    public class TenantCreatedConsumer : IConsumer<TenantCreated>
    {
        private readonly ILogger log;

        public TenantCreatedConsumer(ILogger<TenantCreatedConsumer> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Consume(ConsumeContext<TenantCreated> context)
        {
            log.LogInformation($"Tenant created with id: '{context.Message.Id}' on: '{context.Message.CreationTime.ToString()}'");
            // throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
