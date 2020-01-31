using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Data.App;

namespace WebApplication1.Data
{
    internal interface ISystemControllerRepository
    {
        //Task<bool> Add(CreateControllerDto controller);
        Task<SbcController> Add(SbcController controller, CancellationToken cancellationToken);
        Task Delete(SbcController controller, CancellationToken cancellationToken);
        Task<SbcController> Get(long controllerId, CancellationToken cancellationToken);
        Task<ICollection<SbcController>> GetPage(int page, int count, CancellationToken cancellationToken);
        Task<(int totalCount, int totalPages)> GetTotalPages(int count, CancellationToken cancellationToken);
        Task<SbcController> Update(SbcController controller, CancellationToken cancellationToken);

        Task<ResController> Unprotect(SbcController controller, CancellationToken cancellationToken);
        Task<ICollection<ResController>> Unprotect(ICollection<SbcController> controllers, CancellationToken cancellationToken);

        //Task<IEnumerable<ResController>> GetAll();
    }
}