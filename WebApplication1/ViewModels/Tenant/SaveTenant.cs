using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Attributes;

namespace WebApplication1.ViewModels
{
    [IsDefaultConnectionSaveTenant]
    public class SaveTenant
    {
        /// <summary>
        /// The tenants host identifier.
        /// </summary>
        [Required]
        public string Identifier { get; set; }
        /// <summary>
        /// The tenants host readable name.
        /// </summary>
        public string Name { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public bool IsDefaultConnection { get; set; }
        /// <summary>
        /// postgres connection string
        /// </summary>
        public string ConnectionString { get; set; }
    }
}
