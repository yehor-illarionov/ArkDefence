using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public class PutTenantCommand : IPutTenantCommand
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper;
        private readonly IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper;
        private readonly IHostTenantInfo host;

        public PutTenantCommand(
            ITenantRepository tenantRepository, 
            IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper, 
            IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper,
            IHostTenantInfo host)
        {
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
            this.tenantToTenantMapper = tenantToTenantMapper ?? throw new ArgumentNullException(nameof(tenantToTenantMapper));
            this.saveTenantToTenantMapper = saveTenantToTenantMapper ?? throw new ArgumentNullException(nameof(saveTenantToTenantMapper));
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public async Task<IActionResult> ExecuteAsync(
            string tenantId, 
            SaveTenant saveTenant, 
            CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.Get(tenantId, cancellationToken);
            if(tenant is null)
            {
                return new NotFoundResult();
            }
            if (saveTenant.IsDefaultConnection == false)
            {
                if (this.TestConnection(host.TenantInfo(), saveTenant.ConnectionString) == false)
                {
                    return new StatusCodeResult(417);//Expectation failed
                }
            }
            saveTenantToTenantMapper.Map(saveTenant, tenant);
            tenant = await tenantRepository.Update(tenant, cancellationToken);
            var tenantViewModel = tenantToTenantMapper.Map(tenant);
            return new OkObjectResult(tenantViewModel);
        }
    }
}
