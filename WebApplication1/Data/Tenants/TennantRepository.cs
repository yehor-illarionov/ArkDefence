using Core.Domain;
using Enexure.MicroBus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Data
{
    //public class TennantRepository
    //{
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
    //}
}
