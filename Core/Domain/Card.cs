using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Domain
{
    public class Card : Entity<string>
    {
        public Card(string id):base(id)
        {
          //  Id = id ?? throw new ArgumentNullException(nameof(id));
           // CreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Uuid
        /// </summary>
       // [Key]
       // public string Id { get; set; }
       // public DateTime CreationTime { get; set; }
       // public bool Deleted { get; set; }
       // public DateTime DeletionTime { get; set; }

        public long PersonId { get; set; }
        public Person Person { get; set; }
    }
}
