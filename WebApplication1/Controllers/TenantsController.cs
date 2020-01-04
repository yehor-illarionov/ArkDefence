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
using WebApplication1.ViewModels;
using Microsoft.AspNetCore.JsonPatch;

namespace WebApplication1.Controllers
{
    [Area("api/system")]
    [Route("[area]/[controller]", Name=ControllerName.Tenant)]
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
        [HttpDelete("{tenantId}", Name = TenantControllerRoute.DeleteTenant)]
        [SwaggerResponse(StatusCodes.Status204NoContent, "The tenant with the specified unique identifier was deleted.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A tenant with the specified unique identifier was not found.")]
        public Task<IActionResult> Delete(
            [FromServices] IDeleteTenantCommand command,
            string tenantId,
            CancellationToken cancellationToken) => command.ExecuteAsync(tenantId, cancellationToken);

        /// <summary>
        /// Gets the tenant with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="tenantId">The tenants unique identifier.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the tenant or a 404 Not Found if a tenant with the specified unique
        /// identifier was not found.</returns>
        [HttpGet("{tenantId}", Name = TenantControllerRoute.GetTenant)]
        [HttpHead("{tenantId}", Name = TenantControllerRoute.HeadTenant)]
        [SwaggerResponse(StatusCodes.Status200OK, "The tenant with the specified unique identifier.", typeof(ViewModels.Tenant))]
        [SwaggerResponse(StatusCodes.Status304NotModified, "The tenant has not changed since the date given in the If-Modified-Since HTTP header.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A tenant with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> Get(
            [FromServices] IGetTenantRequest command,
            string tenantId,
            CancellationToken cancellationToken)
        {
            //Console.WriteLine(ControllerContext.ActionDescriptor.AttributeRouteInfo.Name);
            return command.ExecuteAsync(tenantId, cancellationToken); 
        }

        /// <summary>
        /// Gets a collection of tenants using the specified page number and number of items per page.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="pageOptions">The page options.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing a collection of tenants, a 400 Bad Request if the page request
        /// parameters are invalid or a 404 Not Found if a page with the specified page number was not found.
        /// </returns>
        [HttpGet("", Name = TenantControllerRoute.GetTenantPage)]
        [HttpHead("", Name = TenantControllerRoute.HeadTenantPage)]
        [SwaggerResponse(StatusCodes.Status200OK, "A collection of cars for the specified page.", typeof(PageResultTenant))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The page request parameters are invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A page with the specified page number was not found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        public Task<IActionResult> GetPage(
            [FromServices] IGetTenantPageRequest command,
            [FromQuery] PageOptions pageOptions,
            CancellationToken cancellationToken) => command.ExecuteAsync(pageOptions, cancellationToken);

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="tenant">The tenant to create.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 201 Created response containing the newly created tenant or a 400 Bad Request if the tenant is
        /// invalid.</returns>
        [HttpPost("", Name = TenantControllerRoute.PostTenant)]
        [SwaggerResponse(StatusCodes.Status201Created, "The tenant was created.", typeof(ViewModels.Tenant))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The tenant is invalid.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        [SwaggerResponse(StatusCodes.Status409Conflict, "The tenant already exists.")]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed, "Postgres cannot connect using provided connectionString.")]
        public Task<IActionResult> Post(
            [FromServices] IPostTenantCommand command,
            [FromBody] SaveTenant tenant,
            CancellationToken cancellationToken) => command.ExecuteAsync(tenant);

        /// <summary>
        /// Updates an existing tenant with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="tenantId">The car identifier.</param>
        /// <param name="tenant">The car to update.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK response containing the newly updated tenant, a 400 Bad Request if the tenant is invalid or a
        /// or a 404 Not Found if a tenant with the specified unique identifier was not found.</returns>
        [HttpPut("{tenantId}", Name = TenantControllerRoute.PutTenant)]
        [SwaggerResponse(StatusCodes.Status200OK, "The tenant was updated.", typeof(ViewModels.Tenant))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The tenant is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A tenant with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed, "Postgres cannot connect using provided connectionString.")]
        public Task<IActionResult> Put(
            [FromServices] IPutTenantCommand command,
            string tenantId,
            [FromBody] SaveTenant tenant,
            CancellationToken cancellationToken) => command.ExecuteAsync(tenantId, tenant, cancellationToken);

        /// <summary>
        /// Patches the tenant with the specified unique identifier.
        /// </summary>
        /// <param name="command">The action command.</param>
        /// <param name="tenantId">The cars unique identifier.</param>
        /// <param name="patch">The patch document. See http://jsonpatch.com.</param>
        /// <param name="cancellationToken">The cancellation token used to cancel the HTTP request.</param>
        /// <returns>A 200 OK if the tenant was patched, a 400 Bad Request if the patch was invalid or a 404 Not Found
        /// if a tenant with the specified unique identifier was not found.</returns>
        [HttpPatch("{tenantId}", Name = TenantControllerRoute.PatchTenant)]
        [SwaggerResponse(StatusCodes.Status200OK, "The patched tenant with the specified unique identifier.", typeof(ViewModels.Tenant))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "The patch document is invalid.")]
        [SwaggerResponse(StatusCodes.Status404NotFound, "A tenant with the specified unique identifier could not be found.")]
        [SwaggerResponse(StatusCodes.Status406NotAcceptable, "The specified Accept MIME type is not acceptable.")]
        [SwaggerResponse(StatusCodes.Status417ExpectationFailed, "Postgres cannot connect using provided connectionString.")]
        public Task<IActionResult> Patch(
            [FromServices] IPatchTenantCommand command,
            string tenantId,
            [FromBody] JsonPatchDocument<SaveTenant> patch,
            CancellationToken cancellationToken) => command.ExecuteAsync(tenantId, patch,cancellationToken);
    }
} 