using Finbuckle.MultiTenant;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Consumers
{
    public class TenantCreatedNotDefaultConsumer : IConsumer<TenantCreatedNotDefault>
    {
        private readonly ILogger log;
        private readonly IMultiTenantStore store;

        public TenantCreatedNotDefaultConsumer(ILogger<TenantCreatedConsumer> log, IMultiTenantStore context)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
            this.store = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Consume(ConsumeContext<TenantCreatedNotDefault> context)
        {
            //TODO exceptions
            log.LogInformation($"NonDefault Tenant created with id: '{context.Message.Id}' on: '{context.Message.CreationTime.ToString()}'");
            // throw new NotImplementedException();
            var tenantInfo = await store.TryGetAsync(context.Message.Id);
            if (tenantInfo is null)
            {
                log.LogInformation($"Failed to prepare database for Tenant with id: '{context.Message.Id}' on: '{context.Message.CreationTime.ToString()}'");
                return;
            }
            var builder = new DbContextOptionsBuilder<NextAppContext>().UseNpgsql(tenantInfo.ConnectionString);
            using (var db = new NextAppContext(tenantInfo, builder.Options))
            {
                db.Database.EnsureCreated();
                db.Database.Migrate();
            }
            log.LogInformation($"Prepared database for Tenant with id: '{context.Message.Id}' on: '{context.Message.CreationTime.ToString()}'");
        }
    }
}
