using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Domain;
using Microsoft.EntityFrameworkCore.Design;
using WebApplication1.Data.App;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControllerVersionIndex>();
            modelBuilder.Entity<TerminalVersionIndex>();

            modelBuilder.Entity<SbcController>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<Site>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<Hardware>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<HardwareVersionIndex>().HasQueryFilter(c => !c.Deleted);
           // modelBuilder.Entity<ControllerVersionIndex>().HasQueryFilter(c => !c.Deleted);
            //modelBuilder.Entity<TerminalVersionIndex>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<App.Terminal>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<TerminalConfig>().HasQueryFilter(c => !c.Deleted);
            modelBuilder.Entity<TerminalConfigTemplate>().HasQueryFilter(c => !c.Deleted);

            modelBuilder.Entity<Hardware>().Property(p => p.Type).HasConversion<int>();
            modelBuilder.Entity<TerminalConfigTemplate>().Property(p => p.Mode).HasConversion<int>();

            modelBuilder.Entity<TerminalConfig>().HasKey(c => c.Id);
            modelBuilder.Entity<TerminalConfig>().HasOne(tc => tc.TerminalConfigTemplate).WithMany(c => c.TerminalConfigs).HasForeignKey(bc=>bc.ConfigId);
            modelBuilder.Entity<TerminalConfig>().HasOne(tc => tc.Terminal).WithMany(t => t.TerminalConfigs).HasForeignKey(bc=>bc.TerminalId);
            modelBuilder.Entity<TerminalConfig>().HasIndex(bc => new { bc.TerminalId, bc.ConfigId }).IsUnique();

            modelBuilder.EnableAutoHistory(changedMaxLength: null);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<SbcController> Controllers { get;set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }
        public DbSet<HardwareVersionIndex> HardwareVersionIndices { get; set; }
        public DbSet<App.Terminal> Terminals { get; set; }
        public DbSet<TerminalConfig> TerminalConfigs { get; set; }
        public DbSet<TerminalConfigTemplate> TerminalConfigTemplates { get; set; }

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
