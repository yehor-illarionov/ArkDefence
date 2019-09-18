using System;

namespace Core.Domain
{
    public interface ISoftDelete
    {
        bool Deleted { get; set; }
        DateTime DeletionTime { get; set; }
    }
}
