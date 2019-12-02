using Boxed.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1.Mappers
{
    public class TenantToSaveTenantMapper : IMapper<Data.Tenant, ViewModels.SaveTenant>
    {
        public void Map(Data.Tenant source, SaveTenant destination)
        {
            throw new NotImplementedException();
        }
    }
}
