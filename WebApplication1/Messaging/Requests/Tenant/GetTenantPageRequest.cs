using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Boxed.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using WebApplication1.Constants;
using WebApplication1.Data;
using WebApplication1.Repositories;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public class GetTenantPageRequest : IGetTenantPageRequest
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper<Data.Tenant, ViewModels.Tenant> tenantMapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public GetTenantPageRequest(
            ITenantRepository tenantRepository, 
            IMapper<Data.Tenant, ViewModels.Tenant> tenantMapper, 
            IHttpContextAccessor httpContextAccessor,
            LinkGenerator linkGenerator)
        {
            this.tenantRepository = tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
            this.tenantMapper = tenantMapper ?? throw new ArgumentNullException(nameof(tenantMapper));
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
        }

        public async Task<IActionResult> ExecuteAsync(PageOptions parameter, CancellationToken cancellationToken = default)
        {
            var tenants = await tenantRepository.GetPage(parameter.Page.Value ,parameter.Count.Value, cancellationToken).ConfigureAwait(false);
            if(tenants is null)
            {
                return new NotFoundResult();
            }
            var (totalCount, totalPages) = await tenantRepository.GetTotalPages(parameter.Count.Value, cancellationToken).ConfigureAwait(false);
            var tenantViewModels = tenantMapper.MapList(tenants);
            var page = new PageResultTenant()
            {
                Count=tenants.Count,
                Items=tenantViewModels,
                Page=parameter.Page.Value,
                TotalCount=totalCount,
                TotalPages=totalPages
            };
            // Add the Link HTTP Header to add URL's to next, previous, first and last pages.
            // See https://tools.ietf.org/html/rfc5988#page-6
            // There is a standard list of link relation types e.g. next, previous, first and last.
            // See https://www.iana.org/assignments/link-relations/link-relations.xhtml
            this.httpContextAccessor.HttpContext.Response.Headers.Add(
                "Link",
                this.GetPageLinkValue(
                    generator: linkGenerator,
                    pathBase: httpContextAccessor.HttpContext.Request.PathBase,
                    actionRouteName: TenantControllerRoute.GetTenantPage,
                    page: page));

            return new OkObjectResult(page);
        }
    }
}
