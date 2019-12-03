using Enexure.MicroBus;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Handlers
{
    public class TenantCreatedNotDefaultHandler : IEventHandler<TenantCreated>
    {
        private readonly ILogger log;
        private readonly IMultiTenantStore context;

        public TenantCreatedNotDefaultHandler(ILogger<TenantCreatedHandler> log, IMultiTenantStore context)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Handle(TenantCreated @event)
        {
            //TODO exceptions
            log.LogInformation($"NonDefault Tenant created with id: '{@event.Id}' on: '{@event.CreationTime.ToString()}'");
            // throw new NotImplementedException();
            var tenantInfo=await context.TryGetAsync(@event.Id);
            if(tenantInfo is null)
            {
                log.LogInformation($"Failed to prepare database for Tenant with id: '{@event.Id}' on: '{@event.CreationTime.ToString()}'");
                return;
            }
            var builder = new DbContextOptionsBuilder<NextAppContext>().UseNpgsql(tenantInfo.ConnectionString);
            using(var db = new NextAppContext(tenantInfo, builder.Options))
            {
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }
            log.LogInformation($"Prepared database for Tenant with id: '{@event.Id}' on: '{@event.CreationTime.ToString()}'");
        }
    }
}
