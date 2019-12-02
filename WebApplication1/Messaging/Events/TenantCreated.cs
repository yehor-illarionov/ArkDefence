using Enexure.MicroBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Messaging.Events
{
    public class TenantCreated : IEvent
    {
        public TenantCreated(string id, DateTime creationTime)
        {
            Id = id;
            CreationTime = creationTime;
        }

        public string Id { get; private set; }
        public DateTime CreationTime { get; private set; }
    }
}
