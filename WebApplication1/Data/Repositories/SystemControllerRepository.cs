using Core.Domain;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data.App;
using WebApplication1.Exceptions;

namespace WebApplication1.Data
{
    internal sealed class SystemControllerRepository : ISystemControllerRepository
    {
        private readonly NextAppContext context;
        private readonly IDataProtectionProvider provider;
        private readonly IDataProtector protector;
        private readonly TenantInfo tenantInfo;
        private const string Host = "Host";
        public SystemControllerRepository(TenantInfo tenantInfo, IDataProtectionProvider provider)
        {
            this.tenantInfo = tenantInfo ?? throw new ArgumentNullException(nameof(tenantInfo));
            var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            optionsBuilder.UseNpgsql("Server=127.0.0.1;Port=5432;Database=arkdefence;User Id=postgres;Password=BmHkYi5436!;");//TODO dynamic
            this.context = new NextAppContext(tenantInfo, optionsBuilder.Options);
            this.provider = provider ?? throw new ArgumentNullException(nameof(provider));
            protector = provider.CreateProtector("App.v1");//TODO dynamic protector
        }

        public async Task<bool> Add(CreateControllerDto controller)
        {
            if (tenantInfo.Name != Host)
            {
                //throw new HttpResponseException() { Status = 403, Value = "Only Host allowed to add Controllers" };
                return false;
            }
            var ClientIdHash = protector.Protect(controller.ClientId);
            var ClientSecretHash = protector.Protect(controller.ClientSecret);
            //var hostTenant = await context.TryGetAsync("tenant-host").ConfigureAwait(false);
            //var optionsBuilder = new DbContextOptionsBuilder<NextAppContext>();
            //optionsBuilder.UseNpgsql(hostTenant.ConnectionString);
            //var db = new NextAppContext(hostTenant, optionsBuilder.Options);

            var newController = new SbcController(controller.ALias);
            newController.ClientIdHash = ClientIdHash;
            newController.ClientSecretHash = ClientSecretHash;

            context.EnsureAutoHistory();
            context.Controllers.Add(newController);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ResController>> GetAll()
        {
            var sbcs = context.Controllers.AsNoTracking().Include(c => c.Terminals).ToList();
            var res = new List<ResController>();
            if (tenantInfo.Name != Host)
            {
                foreach (var item in sbcs)
                {
                    var temp = new ResController(item);
                    //temp.Mac = item.Mac;
                    res.Add(temp);
                }
            }
            else
            {
                foreach (var item in sbcs)
                {
                    var temp = new ResController(item);
                    //temp.Mac = item.Mac;
                    temp.ClientId = protector.Unprotect(item.ClientIdHash);
                    temp.ClientSecret = protector.Unprotect(item.ClientSecretHash);
                    res.Add(temp);
                }
            }
            return res;
        }

        public async Task<bool> AddTerminal(long controllerId, long terminalId)
        {
            context.EnsureAutoHistory();
            var controller = context.Controllers.Where(c => c.Id == controllerId).Include(c => c.Terminals).FirstOrDefault();
            if (!(controller is null))
            {
                if (controller.Terminals is null)
                {
                    controller.Terminals = new List<App.Terminal>();
                }
                var terminal = context.Terminals.Where(t => t.Id == terminalId).Include(c => c.Controller).FirstOrDefault();
                if (!(terminal is null))
                {
                    if (!(terminal.Controller is null))
                    {
                        return false;
                    }
                    else
                    {
                        controller.Terminals.Add(terminal);
                        //terminal.Controller = controller;
                        await context.SaveChangesAsync();
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
        public ResController() { }
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

    public class CreateControllerDto
    {
        [Required]
        public string ClientSecret { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string ALias { get; set; }
    }
}
