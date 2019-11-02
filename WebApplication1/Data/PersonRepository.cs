using Core.Domain;
using Enexure.MicroBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Messaging.Events;

namespace WebApplication1.Data
{
    //public class PersonRepository
    //{
    //    private readonly ApplicationDbContext context;
    //    private readonly IMicroBus bus;

    //    public PersonRepository(ApplicationDbContext context, IMicroBus bus)
    //    {
    //        this.context = context ?? throw new ArgumentNullException(nameof(context));
    //        this.bus =  bus ?? throw new ArgumentNullException(nameof(bus));
    //    }

    //    public async Task AddAsync(Person person)
    //    {
    //        await context.AddAsync(person);
    //        await bus.PublishAsync(new PersonCreated(person.Id, person.CreationTime));
    //    }
    //}
}
