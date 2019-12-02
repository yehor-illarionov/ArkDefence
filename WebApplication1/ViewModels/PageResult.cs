using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModelSchemaFilters;

namespace WebApplication1.ViewModels
{
    public class PageResult<T> where T : class
    {
        public int Page { get; set; }

        public int Count { get; set; }

        public bool HasNextPage { get => this.Page < this.TotalPages; }

        public bool HasPreviousPage { get => this.Page > 1; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; }

    }

    //[SwaggerSchemaFilter(typeof(PageResultTenantSchemaFilter))]
    public class PageResultTenant : PageResult<ViewModels.Tenant>
    {

    }
}
