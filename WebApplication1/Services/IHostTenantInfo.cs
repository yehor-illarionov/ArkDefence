using Finbuckle.MultiTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface IHostTenantInfo
    {
        public TenantInfo TenantInfo();
    }
}
