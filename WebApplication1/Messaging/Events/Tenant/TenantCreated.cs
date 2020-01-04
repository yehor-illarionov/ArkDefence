using MassTransit;
using System;

namespace WebApplication1.Messaging.Events
{
    /// <summary>
    /// тенанта создан с дефолтной базой данных
    /// </summary>
    public interface TenantCreated : CorrelatedBy<Guid>
    {
        public string Id { get; }
        public DateTime CreationTime { get; }
    }
}
