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
    public class PageResultTenantSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var pageResult = new PageResult<ViewModels.Tenant>()
            {
                Count = 2,
                Items = new List<ViewModels.Tenant>()
                {
                    new Tenant
                    {
                        Id="tenant-id",
                        Identifier="some",
                        Name="Some",
                        Email="example@com",
                        Phone="45654565",
                        Url="/api/system/tenants/tenant-id"
                    },
                    new Tenant
                    {
                        Id="tenant-id2",
                        Identifier="new",
                        Name="New",
                        Email="example2@com",
                        Phone="12312345",
                        Url="/api/system/tenants/tenant-id2"
                    }
                },
                Page = 1,
                TotalCount = 50,
                TotalPages = 10,
            };
            schema.Default = (IOpenApiAny)pageResult;
            schema.Example = (IOpenApiAny)pageResult;
        }
    }
}
