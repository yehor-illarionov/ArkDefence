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
    public class GetTenantPageCommand : IGetTenantPageCommand
    {
        private readonly ITenantRepository tenantRepository;
        private readonly IMapper<Data.Tenant, ViewModels.Tenant> tenantMapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly LinkGenerator linkGenerator;

        public GetTenantPageCommand(
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
            var tenants = await tenantRepository.GetPage(parameter.Page.Value ,parameter.Count.Value, cancellationToken);
            if(tenants is null)
            {
                return new NotFoundResult();
            }
            var (totalCount, totalPages) = await tenantRepository.GetTotalPages(parameter.Count.Value, cancellationToken);
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
                this.GetLinkValue(page));

            return new OkObjectResult(page);
        }

        private string GetLinkValue(PageResultTenant page)
        {
            var values = new List<string>(4);
            if (page.HasNextPage)
            {
                values.Add(GetLinkValueItem("next", page.Page + 1, page.Count));
            }
            if (page.HasPreviousPage)
            {
                values.Add(GetLinkValueItem("previous", page.Page - 1, page.Count));
            }
            values.Add(this.GetLinkValueItem("first", 1, page.Count));
            values.Add(this.GetLinkValueItem("last", page.TotalPages, page.Count));
            return string.Join(", ", values);
        }

        private string GetLinkValueItem(string rel, int page, int count)
        {
            var url = linkGenerator.GetPathByRouteValues(
                TenantControllerRoute.GetTenantPage, 
                new PageOptions { Page = page, Count = count },
                httpContextAccessor.HttpContext.Request.PathBase);
            return $"<{url}>; rel=\"{rel}\"";
        }
    }
}
