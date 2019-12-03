using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using Enexure.MicroBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly IHostTenantInfo host;

        public PostTenantCommand(
            ITenantRepository tenantRepository, 
            IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper, 
            IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper,
            IMicroBus bus,
            IHostTenantInfo host)
        {
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
            this.tenantToTenantMapper = tenantToTenantMapper ?? throw new ArgumentNullException(nameof(tenantToTenantMapper));
            this.saveTenantToTenantMapper = saveTenantToTenantMapper ?? throw new ArgumentNullException(nameof(saveTenantToTenantMapper));
            this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
            host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public async Task<IActionResult> ExecuteAsync(SaveTenant parameter, CancellationToken cancellationToken = default)
        {
            if (parameter.IsDefaultConnection)
            {
                parameter.ConnectionString = host.TenantInfo().ConnectionString;
            }
            else
            {
                if (this.TestConnection(host.TenantInfo(), parameter.ConnectionString)== false)
                {
                    return new StatusCodeResult(417);//Expectation failed
                }
            }

            var tenant = saveTenantToTenantMapper.Map(parameter);
            tenant = await tenantRepository.Add(tenant, out RepositoryError error,cancellationToken);
            if (error == RepositoryError.AlreadyExists)
            {
                return new ConflictResult();
            }
            if (error != RepositoryError.None)
            {
                return new StatusCodeResult(500);
            }
            var tenantViewModel = tenantToTenantMapper.Map(tenant);

            if (parameter.IsDefaultConnection)
            {
                await bus.PublishAsync(new TenantCreated(tenant.Id, tenant.Created.UtcDateTime));
            }
            else
            {
                await bus.PublishAsync(new TenantCreatedNotDefault(tenant.Id, tenant.Created.UtcDateTime));
            }
           
            return new CreatedAtRouteResult(
                TenantControllerRoute.GetTenant,
                new { tenantId = tenantViewModel.Id },
                tenantViewModel);
        }

    }
}
