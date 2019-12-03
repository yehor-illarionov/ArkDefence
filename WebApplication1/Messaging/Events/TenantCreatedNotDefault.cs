using Enexure.MicroBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Messaging.Events
{
    public class TenantCreatedNotDefault : IEvent
    {
        public TenantCreatedNotDefault(string id, DateTime creationTime)
        {
            Id = id;
            CreationTime = creationTime;
        }

        public string Id { get; private set; }
        public DateTime CreationTime { get; private set; }
    }
}
