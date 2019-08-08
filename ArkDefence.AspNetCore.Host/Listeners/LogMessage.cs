using ArkDefence.AspNetCore.Host.Data;
using ArkDefence.AspNetCore.Host.Models;
using ArkDefence.AspNetCore.Host.Models.Events;
using Coravel.Events.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArkDefence.AspNetCore.Host.Listeners
{
    public class LogMessage : IListener<MessageReceived>
    {
        private readonly ApplicationDbContext _dbcontext;

        public LogMessage(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext));
        }

        public async Task HandleAsync(MessageReceived broadcasted)
        {
            _dbcontext.Add<MessageHistory>(new MessageHistory(broadcasted.Method, $"user:{broadcasted.UserIdentifier}; data:[{broadcasted.Data}]"));
            Console.WriteLine($"Message Log. user:{broadcasted.UserIdentifier} method:{broadcasted.Method}");
            await _dbcontext.SaveChangesAsync();
        }
    }
}
