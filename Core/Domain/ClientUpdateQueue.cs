using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class ClientUpdateQueue
    {
        [Key]
        public long Id { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsFailed { get; set; }
        public ActionType ActionType { get; set; }
        public EntityType EntityType { get; set; }
        public string Data { get; set; }
        public string Reason { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CompletionTime { get; set; }
        public string SystemControllerId { get; set; }
        public string TennantId { get; set; }
    }

    public enum ActionType
    {
        Add,
        Update,
        Remove,
    }
    public enum EntityType
    {
        Terminal,
        SystemController,
        Person,
        Fingerprint,
        Card,
        Ble
    }
}
