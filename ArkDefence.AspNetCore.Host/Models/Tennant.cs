using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class Tennant:ISoftDelete
    {
        public Tennant(string key)
        {
            Id = key ?? throw new ArgumentException(nameof(key));
            CreationTime = DateTime.UtcNow;
            SystemControllers = new List<SystemController>();
        }
        [Key]
        public string Id { get; set; }
        public string Alias { get; set; }
        [EmailAddress]
        public string Email { get; set; }
       // [Phone]
        public string Phone { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }

        public DateTime CreationTime { get; set; }
        /// <summary>
        /// soft delete
        /// </summary>
        public bool Deleted { get; set; }
        public DateTime DeletionTime { get; set; }

        [InverseProperty("Tennant")]
        public List<SystemController> SystemControllers { get; set; }

        [InverseProperty("Tennant")]
        public List<Person> Users { get; set; }
    }
}
