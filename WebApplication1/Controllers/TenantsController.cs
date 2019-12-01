using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Exceptions;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.Net.Http.Headers;
using WebApplication1.Constants;
using System.Threading;
using WebApplication1.Commands;

namespace WebApplication1.Controllers
{
    [Route("/api/system/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase{
        /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods.
        /// </summary>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options()
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Post);
            return this.Ok();
        }

           /// <summary>
        /// Returns an Allow HTTP header with the allowed HTTP methods for a tenant with the specified unique identifier.
        /// </summary>
        /// <param name="tenantId">The tetant unique identifier.</param>
        /// <returns>A 200 OK response.</returns>
        [HttpOptions("{tenantId}")]
        [SwaggerResponse(StatusCodes.Status200OK, "The allowed HTTP methods.")]
        public IActionResult Options(string tenantId)
        {
            this.HttpContext.Response.Headers.AppendCommaSeparatedValues(
                HeaderNames.Allow,
                HttpMethods.Delete,
                HttpMethods.Get,
                HttpMethods.Head,
                HttpMethods.Options,
                HttpMethods.Patch,
                HttpMethods.Post,
                HttpMethods.Put);
            return this.Ok();
        }

        /// <summary>
        /// Deletes the tenant with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="tenantId">The tenant unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 204 No Content response if the tenant was deleted or a 404 Not Found if a tenant with the specified
        /// unique identifier was not found.</returns>
        [HttpDelete("{tenantId}", Name = TenantsControllerRoute.DeleteTenant)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The tenant with the specified unique identifier was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A tenant with the specified unique identifier was not found.")]
        public Task<IActionResult> Delete(
            [FromServices] IDeleteTenantCommand command,
            string tenantId,
            CancellationToken cancellationToken) => command.ExecuteAsync(tenantId);

        /// <summary>
        /// Gets the tenant with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="tenantId">The tenants unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the tenant or a 404 Not Found if a tenant with the specified unique
        /// identifier was not found.</returns>
        [HttpGet("{tenantId}", Name = TenantsControllerRoute.GetTenant)]
        [HttpHead("{tenantId}", Name = TenantsControllerRoute.HeadTenant)]
        [SwaggerResponse(StatusCodes.Status200OK, "The tenant with the specified unique identifier.", typeof(Tenant))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The tenant has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A tenant with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Get(
            [FromServices] IGetTenantCommand command,
            string tenantId,
            CancellationToken cancellationToken) => command.ExecuteAsync(tenantId);
    }
} 