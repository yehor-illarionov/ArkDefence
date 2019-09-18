using System;
using Enexure.MicroBus;

namespace WebApplication1.Messaging.Events
{
    internal class PersonCreated : IEvent
    {
        private long id;
        private DateTime creationTime;

        public PersonCreated(long id, DateTime creationTime)
        {
            this.id = id;
            this.creationTime = creationTime;
        }
    }
}