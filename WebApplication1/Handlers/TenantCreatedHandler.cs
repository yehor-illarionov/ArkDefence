using Enexure.MicroBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Handlers
{
    public class TenantCreatedHandler : IEventHandler<TenantCreated>
    {
        private readonly ILogger log;

        public TenantCreatedHandler(ILogger<TenantCreatedHandler> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task Handle(TenantCreated @event)
        {
            log.LogInformation($"Tenant created with id: '{@event.Id}' on: '{@event.CreationTime.ToString()}'");
            // throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
