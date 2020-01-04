using Boxed.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Mappers
{
    public class TenantToSaveTenantMapper : IMapper<Data.Tenant, ViewModels.SaveTenant>, IMapper<ViewModels.SaveTenant, Data.Tenant>
    {
        private readonly IClockService clock;

        public TenantToSaveTenantMapper(IClockService clock)
        {
            this.clock = clock ?? throw new ArgumentNullException(nameof(clock));
        }

        public void Map(Data.Tenant source, SaveTenant destination)
        {
            if(source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if(destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            destination.ConnectionString = source.ConnectionString;
            destination.Email = source.Email;
            destination.Identifier = source.Identifier;
            destination.IsDefaultConnection = false;//TODO check
            destination.Name = source.Name;
            destination.Phone = source.Phone;
        }

        public void Map(SaveTenant source, Data.Tenant destination)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (destination is null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            var now = clock.UtcNow;
            if (destination.Created == DateTimeOffset.MinValue)
            {
                destination.Created = now;
            }
            destination.ConnectionString = source.ConnectionString;
            destination.Identifier = source.Identifier;
            destination.Name = source.Name;
            destination.Phone = source.Phone;
            destination.Email = source.Email;
            destination.Modified = now;
        }
    }
}
