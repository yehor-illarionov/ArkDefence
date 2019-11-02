using Core.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Data
{
    public class ControllerRepository
    {
        private readonly IMultiTenantStore context;
        private readonly IDataProtectionProvider provider;
        private readonly IDataProtector protector;

        public ControllerRepository(IMultiTenantStore context, IDataProtectionProvider provider)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            protector=provider.CreateProtector("App.v1");
        }

        public async void AddController(ResController controller)
        {
            controller.ClientIdHash=protector.Protect(controller.ClientId);
            controller.ClientSecretHash = protector.Protect(controller.ClientSecret);

            var hostTenant = await context.TryGetAsync("tenant-host").ConfigureAwait(false);
            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            optionsBuilder.UseNpgsql(hostTenant.ConnectionString);
            var db = new NextAppContext(hostTenant, optionsBuilder.Options);

            var newController = (SbcController)controller;
            db.EnsureAutoHistory();
            db.Controllers.Add(newController);
            await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ResController>> GetAllAsync()
        {
            var hostTenant = await context.TryGetAsync("tenant-host").ConfigureAwait(false);
            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            optionsBuilder.UseNpgsql(hostTenant.ConnectionString);
            var db = new NextAppContext(hostTenant, optionsBuilder.Options);

            var sbcs = db.Controllers.AsNoTracking().Include(c=>c.Terminals).ToList();
            var res = new List<ResController>();
            foreach(var item in sbcs)
            {
                // var temp = new ResController(item.Alias,"","");
                var temp = new ResController(item);
                //temp.Mac = item.Mac;
                temp.ClientId = protector.Unprotect(item.ClientIdHash);
                temp.ClientSecret = protector.Unprotect(item.ClientSecretHash);
                res.Add(temp);
            }
            return res;
        }

        public async Task<bool> AddTerminalAsync(long controllerId, long terminalId)
        {
            var hostTenant = await context.TryGetAsync("tenant-host").ConfigureAwait(false);
            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            optionsBuilder.UseNpgsql(hostTenant.ConnectionString);
            var db = new NextAppContext(hostTenant, optionsBuilder.Options);
            db.EnsureAutoHistory();
            var controller = db.Controllers.Where(c=>c.Id==controllerId).Include(c => c.Terminals).FirstOrDefault();
            if(!(controller is null))
            {
                if(controller.Terminals is null)
                {
                    controller.Terminals = new List<App.Terminal>();
                }
                var terminal = db.Terminals.Where(t => t.Id == terminalId).Include(c => c.Controller).FirstOrDefault();
                if(!(terminal is null))
                {
                    if(!(terminal.Controller is null))
                    {
                        return false;
                    }
                    else
                    {
                        controller.Terminals.Add(terminal);
                        //terminal.Controller = controller;
                        await db.SaveChangesAsync();
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }

    public class ResController : SbcController
    {
        public ResController(string alias, string clientid, string clientSecret) : base(alias)
        {
            ClientId = clientid ?? throw new ArgumentNullException(nameof(clientid));
            ClientSecret=clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
        }

        public ResController(SbcController controller) 
        {
            Alias = controller.Alias;
            Mac = controller.Mac;
            CreationTime = controller.CreationTime;
            DeletionTime = controller.DeletionTime;
            //ClientIdHash = controller.ClientIdHash;
           // ClientSecretHash = controller.ClientSecretHash;
            Id = controller.Id;
            Site = controller.Site;
            Deleted = controller.Deleted;
            VersionIndex = controller.VersionIndex;
            Terminals = controller.Terminals;
        }

        public string ClientSecret { get; set; }
        public string ClientId { get; set; }
    }
}
