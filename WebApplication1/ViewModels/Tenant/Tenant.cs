using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModelSchemaFilters;

namespace WebApplication1.ViewModels
{
   // [SwaggerSchemaFilter(typeof(TenantSchemaFilter))]
    public class Tenant
    {
        /// <summary>
        /// The tenants unique identifier.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The tenants host identifier.
        /// </summary>
        public string Identifier { get; set; }
        /// <summary>
        /// The tenants host readable name.
        /// </summary>
        public string Name { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        /// <summary>
        /// The URL used to retrieve the resource conforming to REST'ful JSON http://restfuljson.org/.
        /// </summary>
        [Url]
        public string Url { get; set; }
    }
}
