using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Finbuckle.MultiTenant;
using WebApplication1.Data;

namespace WebApplication1.Services
{
    public class HostTenantInfo : IHostTenantInfo
    {
        private TenantInfo tenantInfo { get; set; }
        private readonly IMultiTenantStore context;
        private const string id = "tenant-host";

        public HostTenantInfo(IMultiTenantStore context){
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            _ = Init();
        }

        private async Task Init()
        {
            tenantInfo = await context.TryGetAsync(id).ConfigureAwait(false);
        }
        public TenantInfo TenantInfo()
        {
            return new TenantInfo(
                tenantInfo.Id,
                tenantInfo.Identifier,
                tenantInfo.Name,
                tenantInfo.ConnectionString,
                tenantInfo.Items);
        }
    }
}
