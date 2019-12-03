using Boxed.Mapping;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public class PatchTenantCommand : IPatchTenantCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly IObjectModelValidator objectModelValidator;
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper;
        private readonly IMapper<Data.Tenant, SaveTenant> tenantToSaveTenantMapper;
        private readonly IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper;
        private readonly IHostTenantInfo host;

        public PatchTenantCommand(
            IActionContextAccessor actionContextAccessor, 
            IObjectModelValidator objectModelValidator, 
            ITenantRepository tenantRepository, 
            IMapper<Data.Tenant, ViewModels.Tenant> tenantToTenantMapper, 
            IMapper<Data.Tenant, SaveTenant> tenantToSaveTenantMapper, 
            IMapper<SaveTenant, Data.Tenant> saveTenantToTenantMapper,
            IHostTenantInfo host)
        {
            this.actionContextAccessor = actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
            this.objectModelValidator = objectModelValidator ?? throw new ArgumentNullException(nameof(objectModelValidator));
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
            this.tenantToTenantMapper = tenantToTenantMapper ?? throw new ArgumentNullException(nameof(tenantToTenantMapper));
            this.tenantToSaveTenantMapper = tenantToSaveTenantMapper ?? throw new ArgumentNullException(nameof(tenantToSaveTenantMapper));
            this.saveTenantToTenantMapper = saveTenantToTenantMapper ?? throw new ArgumentNullException(nameof(saveTenantToTenantMapper));
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

        public async Task<IActionResult> ExecuteAsync(
            string tenantId, 
            JsonPatchDocument<SaveTenant> patch, 
            CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.Get(tenantId, cancellationToken).ConfigureAwait(false);
            if(tenant is null)
            {
                return new NotFoundResult();
            }
            var saveTenant = tenantToSaveTenantMapper.Map(tenant);
            var modelState = actionContextAccessor.ActionContext.ModelState;
            patch.ApplyTo(saveTenant, modelState);
            objectModelValidator.Validate(
                actionContextAccessor.ActionContext,
                validationState: null,
                prefix: null,
                model: saveTenant);
            if (!modelState.IsValid)
            {
                return new BadRequestObjectResult(modelState);
            }
            saveTenantToTenantMapper.Map(saveTenant, tenant);
            if(this.TestConnection(host.TenantInfo(), tenant.ConnectionString) == false)
            {
                return new StatusCodeResult(417);
            }
            await tenantRepository.Update(tenant, cancellationToken);
            var tenantViewModel = this.tenantToTenantMapper.Map(tenant);
            return new OkObjectResult(tenantViewModel);
        }
    }
}
