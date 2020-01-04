using MassTransit;
using System;

namespace WebApplication1.Messaging.Events
{
    public interface PersonCreated : CorrelatedBy<Guid>
    {
        long Id { get; }
        DateTime CreationTime { get; } 
    }
}