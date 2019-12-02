using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using Enexure.MicroBus;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Constants;
using WebApplication1.Data;
using WebApplication1.Messaging.Events;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public class PostTenantCommand : IPostTenantCommand
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper;
        private readonly IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper;
        private readonly IMicroBus bus;

        public PostTenantCommand(
            ITenantRepository tenantRepository, 
            IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper, 
            IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper,
            IMicroBus bus)
        {
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
            this.tenantToTenantMapper = tenantToTenantMapper ?? throw new ArgumentNullException(nameof(tenantToTenantMapper));
            this.saveTenantToTenantMapper = saveTenantToTenantMapper ?? throw new ArgumentNullException(nameof(saveTenantToTenantMapper));
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        public async Task<IActionResult> ExecuteAsync(SaveTenant parameter, CancellationToken cancellationToken = default)
        {
            var tenant = saveTenantToTenantMapper.Map(parameter);
            tenant = await tenantRepository.Add(tenant, cancellationToken);
            var tenantViewModel = tenantToTenantMapper.Map(tenant);
            await bus.PublishAsync(new TenantCreated(tenant.Id, tenant.Created.UtcDateTime));
            return new CreatedAtRouteResult(
                TenantControllerRoute.GetTenant,
                new { tenantId = tenantViewModel.Id },
                tenantViewModel);
        }
    }
}
