using System;

namespace WebApplication1.Data
{
    public interface ISoftDelete
    {
        bool Deleted { get; set; }
        DateTime DeletionTime { get; set; }
    }
}
