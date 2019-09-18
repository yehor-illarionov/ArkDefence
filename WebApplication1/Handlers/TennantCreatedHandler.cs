using Enexure.MicroBus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Handlers
{
    public class TennantCreatedHandler : IEventHandler<TennantCreated>
    {
        private readonly ILogger log;

        public TennantCreatedHandler(ILogger<TennantCreatedHandler> log)
        {
            this.log = log;
        }

        public Task Handle(TennantCreated @event)
        {
            log.LogInformation($"Tennant created with id: '{@event.Id}' on: '{@event.CreationTime.ToString()}'");
            // throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
