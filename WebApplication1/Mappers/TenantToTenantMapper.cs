using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxed.Mapping;
using Boxed.AspNetCore;
using WebApplication1.Data;
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants;

namespace WebApplication1.Mappers
{
    public class TenantToTenantMapper : IMapper<Data.Tenant, ViewModels.Tenant>
    {
        private readonly IUrlHelper urlHelper;

        public TenantToTenantMapper(IUrlHelper urlHelper)
        {
            this.urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
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
                urlHelper.Link(TenantsControllerRoute.GetTenant,new { source.Id});
        }
    }
}
