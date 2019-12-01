using Core.Domain;
using Enexure.MicroBus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        //    private readonly ApplicationDbContext context;
        //    private readonly IMicroBus bus;

        //    public TennantRepository(ApplicationDbContext context, IMicroBus bus)
        //    {
        //        this.context = context ?? throw new ArgumentNullException(nameof(context));
        //        this.bus = bus ?? throw new ArgumentNullException(nameof(bus));
        //    }

        //    public async Task AddAsync(Tennant tennant)
        //    {
        //        await context.AddAsync(tennant);
        //        await bus.PublishAsync(new TennantCreated(tennant.Id, tennant.CreationTime));
        //    }

        //    public async Task UpdateAsync(Tennant tennant)
        //    {
        //        var temp = await context.ArkDefence_Tennant.FindAsync(tennant.Id);
        //        context.EnsureAutoHistory();
        //        context.Update(tennant);
        //        await context.SaveChangesAsync();
        //    } 

        //    public async Task DisableAsync(string id)
        //    {
        //        var temp=await context.FindAsync<Tennant>(id);
        //        if(temp != null)
        //        {
        //            temp.SoftDelete();
        //            context.EnsureAutoHistory();
        //            await context.SaveChangesAsync();
        //        }
        //    }
        private static readonly List<Tenant> Tenants;
        static TenantRepository()=>
        Tenants=new List<Tenant>()
        {
            new Tenant(){
                Id="tenant-1",
                Identifier="tenant1",
                Name="Tenant1",
                Email="tenant1@example.com",
                Phone="564546564",
                ConnectionString="some_conection"
            },
            new Tenant(){
                Id="tenant-2",
                Identifier="tenant2",
                Name="Tenant2",
                Email="tenant2@example.com",
                Phone="564546564",
                ConnectionString="some_conection"
            },
            new Tenant(){
                Id="tenant-3",
                Identifier="tenant3",
                Name="Tenant3",
                Email="tenant3@example.com",
                Phone="564546564",
                ConnectionString="some_conection"
            },
            new Tenant(){
                Id="tenant-4",
                Identifier="tenant4",
                Name="Tenant4",
                Email="tenant4@example.com",
                Phone="564546564",
                ConnectionString="some_conection"
            },
            new Tenant(){
                Id="tenant-5",
                Identifier="tenant5",
                Name="Tenant5",
                Email="tenant5@example.com",
                Phone="564546564",
                ConnectionString="some_conection"
            },
        };

        public Task<Tenant> Add(Tenant tenant, CancellationToken cancellationToken)
        {
            Tenants.Add(tenant);
            tenant.Id=Tenants.Max(x=>x.Id)+1;
            return Task.FromResult(tenant);
        }

        public Task Delete(Tenant tenant, CancellationToken cancellationToken)
        {
           if(Tenants.Contains(tenant)){
               Tenants.Remove(tenant);
           }
           return Task.CompletedTask;
        }

        public Task<Tenant> Get(string tenantId, CancellationToken cancellationToken)
        {
            var tenant=Tenants.FirstOrDefault(x=>x.Id==tenantId);
            return Task.FromResult(tenant);
        }

        public Task<ICollection<Tenant>> GetPage(int page, int count, CancellationToken cancellationToken)
        {
            var pageTenants=Tenants.Skip(count*(page-1)).Take(count).ToList();
            if(pageTenants.Count==0){
                pageTenants=null;
            }
            return Task.FromResult((ICollection<Tenant>)pageTenants);
        }

        public Task<(int totalCount, int totalPages)> GetTotalPages(int count, CancellationToken cancellationToken)
        {
            var totalPages = (int)Math.Ceiling(Tenants.Count / (double)count);
            return Task.FromResult((Tenants.Count, totalPages));
        }

        public Task<Tenant> Update(Tenant tenant, CancellationToken cancellationToken)
        {
            var existingTenant=Tenants.FirstOrDefault(x=>x.Id.Equals(tenant.Id));
            existingTenant.Identifier=tenant.Identifier;
            existingTenant.Name=tenant.Name;
            existingTenant.Email=tenant.Email;
            existingTenant.Phone=tenant.Phone;
            return Task.FromResult(existingTenant);
        }
    }
}
