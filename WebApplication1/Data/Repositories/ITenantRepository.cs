using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Data;

namespace WebApplication1.Repositories
{
    public interface ITenantRepository
    {
        Task<Tenant> Add(Tenant tenant,out RepositoryError error, CancellationToken cancellationToken);
        Task Delete(Tenant tenant, CancellationToken cancellationToken);
        Task<Tenant> Get(string tenantId, CancellationToken cancellationToken);
        Task<ICollection<Tenant>> GetPage(int page, int count, CancellationToken cancellationToken);
        Task<(int totalCount, int totalPages)> GetTotalPages(int count, CancellationToken cancellationToken);
        Task<Tenant> Update(Tenant tenant,CancellationToken cancellationToken);
    }
}
