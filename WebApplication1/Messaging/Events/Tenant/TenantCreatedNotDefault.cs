using System;

namespace WebApplication1.Messaging.Events
{
    /// <summary>
    /// тенант создан с кастомной базой даных
    /// </summary>
    public interface TenantCreatedNotDefault
    {
        string EventId { get; }
        public string Id { get; }
        public DateTime CreationTime { get; }
    }
}
