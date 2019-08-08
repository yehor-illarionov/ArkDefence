using System;
using System.ComponentModel.DataAnnotations;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class Entity<TKey>: ISoftDelete, ICreationTime
    {
        public Entity()
        {
            CreationTime = DateTime.UtcNow;
            Deleted = false;
        }

        public Entity(TKey id)
        {
            Id = id;
            CreationTime = DateTime.UtcNow;
            Deleted = false;
        }

        [Key]
        public TKey Id { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletionTime { get; set; }

        public void SoftDelete()
        {
            this.Deleted = true;
            this.DeletionTime = DateTime.UtcNow;
        }
    }
}
