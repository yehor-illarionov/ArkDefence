using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Entity<TKey>: ISoftDelete, ICreationTime
    {
        public Entity()
        {
            CreationTime = DateTime.UtcNow;
            Deleted = false;
        }

        /// <summary>
        /// throws ArgumentNullException if id is null
        /// </summary>
        /// <param name="id"></param>
        public Entity(TKey id)
        {
            if(id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
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
