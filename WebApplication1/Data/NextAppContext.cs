using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore.Design;

namespace WebApplication1.Data
{
    public class NextAppContext : MultiTenantDbContext
    {
        public NextAppContext(TenantInfo tenantInfo, DbContextOptions<NextAppContext> options) : base(tenantInfo, options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(ConnectionString);
        }

        public DbSet<SbcController> Controllers { get;set; }

    }

    public class NextAppContextFactory : IDesignTimeDbContextFactory<NextAppContext>
    {
        public NextAppContext CreateDbContext(string[] args)
        {
            // To prep each database uncomment the corresponding line below.
            var tenantInfo = new TenantInfo(null, null, null, "Server=127.0.0.1;Port=5432;Database=arkdefence;User Id=postgres;Password=BmHkYi5436!;", null);
           

            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();

            return new NextAppContext(tenantInfo, optionsBuilder.Options);
        }
    }
}
