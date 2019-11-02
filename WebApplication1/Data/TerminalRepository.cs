using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.App;

namespace WebApplication1.Data
{
    public class TerminalRepository
    {
        private readonly IMultiTenantStore context;

        public TerminalRepository(IMultiTenantStore context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async void Add(Terminal terminal)
        {
            var hostTenant = await context.TryGetAsync("tenant-host").ConfigureAwait(false);
            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            optionsBuilder.UseNpgsql(hostTenant.ConnectionString);
            var db = new NextAppContext(hostTenant, optionsBuilder.Options);
            //TODO validation
            db.EnsureAutoHistory();
            db.Terminals.Add(terminal);
            await db.SaveChangesAsync();
        }

        public async Task<ICollection<Terminal>> GetAllAsync()
        {
            var hostTenant = await context.TryGetAsync("tenant-host").ConfigureAwait(false);
            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            optionsBuilder.UseNpgsql(hostTenant.ConnectionString);
            var db = new NextAppContext(hostTenant, optionsBuilder.Options);

            return await db.Terminals.ToArrayAsync();
        }
    }
}
