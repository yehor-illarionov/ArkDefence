using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class SystemController: Entity<string>
    {
        public SystemController(string id):base(id)
        {
           // Id = id ?? throw new ArgumentException(nameof(id));
            //CreationTime = DateTime.UtcNow;
        }
        /// <summary>
        /// auth0 clientid 
        /// </summary>}
        //[Key]
        //public string Id { get; set; }
        /// <summary>
        /// friendly name
        /// </summary>
        public string Alias { get; set; }
        /// <summary>
        /// auth0 client secret
        /// </summary>
        public string ClientSecret { get; set; }
        public string Version { get; set; }

        //public DateTime CreationTime { get; set; }
        /// <summary>
        /// soft delete
        /// </summary>
        //public bool Deleted { get; set; }
        //public DateTime DeletionTime { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }

        public string TennantId { get; set; }
        public Tennant Tennant { get; set; }

        public List<PersonSystemController> PersonSystemControllers { get; set; }
        [InverseProperty("SystemController")]
        public List<Terminal> Terminals { get; set; }
    }
}
