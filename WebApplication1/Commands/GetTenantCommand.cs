using System;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Net.Http.Headers;
using WebApplication1.Data;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public class GetTenantCommand : IGetTenantCommand
    {
        private readonly IActionContextAccessor actionContextAccessor;
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper<Data.Tenant, ViewModels.Tenant> tenantMapper;

        public GetTenantCommand(IActionContextAccessor actionContextAccessor, ITenantRepository tenantRepository, IMapper<Data.Tenant, ViewModels.Tenant> tenantMapper)
        {
            this.actionContextAccessor = actionContextAccessor ?? throw new ArgumentNullException(nameof(actionContextAccessor));
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
            this.tenantMapper = tenantMapper ?? throw new ArgumentNullException(nameof(tenantMapper));
        }

        public async Task<IActionResult> ExecuteAsync(string parameter, CancellationToken cancellationToken = default)
        {
            var tenant = await tenantRepository.Get(parameter, cancellationToken);
            if(tenant is null)
            {
                return new NotFoundResult();
            }
            var httpContext = actionContextAccessor.ActionContext.HttpContext;
            if (httpContext.Request.Headers.TryGetValue(HeaderNames.IfModifiedSince, out var stringValues))
            {
                if (DateTimeOffset.TryParse(stringValues, out var modifiedSince) &&
                    (modifiedSince >= tenant.Modified))
                {
                    return new StatusCodeResult(StatusCodes.Status304NotModified);
                }
            }
            var tenantViewModel = tenantMapper.Map(tenant);
            Console.WriteLine($"Generetade URI:'{tenantViewModel.Url}'");
            httpContext.Response.Headers.Add(HeaderNames.LastModified, tenant.Modified.ToString("R"));
            return new OkObjectResult(tenantViewModel);
        }
    }
}
