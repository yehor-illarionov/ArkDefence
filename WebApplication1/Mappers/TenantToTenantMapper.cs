using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxed.Mapping;
using Boxed.AspNetCore;
using WebApplication1.Data;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Routing;
using WebApplication1.Constants;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Mappers
{
    public class TenantToTenantMapper : IMapper<Data.Tenant, ViewModels.Tenant>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TenantToTenantMapper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public void Map(Data.Tenant source, ViewModels.Tenant destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Id = source.Id;
            destination.Identifier = source.Identifier;
            destination.Name = source.Name;
            destination.Phone = source.Phone;
            destination.Email = source.Email;
            destination.Url =
              linkGenerator.GetPathByRouteValues(
                  TenantControllerRoute.GetTenant, 
                  new { tenantId = source.Id }, 
                  httpContextAccessor.HttpContext.Request.PathBase);
        }

        
    }
}
