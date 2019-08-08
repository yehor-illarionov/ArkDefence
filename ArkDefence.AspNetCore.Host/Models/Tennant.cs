using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Models
{
    public class Tennant:Entity<string>
    {
        public Tennant(string id):base(id)
        {
           // SystemControllers = new List<SystemController>();
           // Users = new List<Person>();
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
