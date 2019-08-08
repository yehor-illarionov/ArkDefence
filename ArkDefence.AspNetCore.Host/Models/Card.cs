using System;
using System.ComponentModel.DataAnnotations;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class Card : ISoftDelete
    {
        public Card(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            CreationTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Uuid
        /// </summary>
        [Key]
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        public bool Deleted { get; set; }
        public DateTime DeletionTime { get; set; }

        public string PersonId { get; set; }
        public Person Person { get; set; }
    }
}
