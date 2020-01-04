namespace WebApplication1.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WebApplication1.Repositories;

    public class DeleteTenantCommmand : IDeleteTenantCommand
    {
        private readonly ITenantRepository tenantRepository; 

        public DeleteTenantCommmand(ITenantRepository tenantRepository){
            this.tenantRepository=tenantRepository ?? throw new ArgumentNullException(nameof(tenantRepository));
        }
        public async Task<IActionResult> ExecuteAsync(string parameter, CancellationToken cancellationToken = default)
        {
            var tenant=await this.tenantRepository.Get(parameter, cancellationToken).ConfigureAwait(false);
            if(tenant is null){
                return new NotFoundResult();
            }
            await this.tenantRepository.Delete(tenant, cancellationToken).ConfigureAwait(false);
            return new NoContentResult();
        }
    }
}