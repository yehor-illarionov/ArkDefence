using Boxed.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public interface IPutTenantCommand : IAsyncCommand<string, SaveTenant>
    {
    }
}
