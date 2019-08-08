using System;

namespace ArkDefence.AspNetCore.Host.Models
{
    public interface ISoftDelete
    {
        bool Deleted { get; set; }
        DateTime DeletionTime { get; set; }
    }
}
