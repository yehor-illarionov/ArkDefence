using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1.ViewModelSchemaFilters
{
    public class TenantSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var tenant = new Tenant()
            {
                Id = "tenant-some",
                Identifier="some",
                Name="Some",
                Email="some@example.com",
                Phone="+380984563214"
            };
            schema.Default = (IOpenApiAny)tenant;
            schema.Example = (IOpenApiAny)tenant;
        }
    }
}
