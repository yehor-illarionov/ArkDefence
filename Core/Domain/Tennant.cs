using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Domain
{
    public class Tennant:Entity<string>
    {
        public Tennant() : base()
        {
            Id = "tennant-" + Guid.NewGuid().ToString();
        }
        /// <summary>
        /// if IsNullOrEmpty or IsNullOrWhiteSpace
        /// creates new id of type 'tennant-guid_string'
        /// </summary>
        /// <param name="id"></param>
        public Tennant(string id):base()
        {
            if (string.IsNullOrEmpty(Id) || string.IsNullOrWhiteSpace(Id))
            {
                Id = "tennant-" + Guid.NewGuid().ToString();
            }
            Id = id;
        }
        public string Alias { get; set; }
        [EmailAddress]
        public string Email { get; set; }
       // [Phone]
        public string Phone { get; set; }
        [Timestamp]
        public byte[] Timestamp { get; set; }


        [InverseProperty("Tennant")]
        public List<SystemController> SystemControllers { get; set; }

        [InverseProperty("Tennant")]
        public List<Person> Users { get; set; }
    }
}
