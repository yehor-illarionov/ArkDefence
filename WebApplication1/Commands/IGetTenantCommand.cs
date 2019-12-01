using System;
using Boxed.AspNetCore;

namespace WebApplication1.Commands
{
    public interface IGetTenantCommand : IAsyncCommand<string>
    {
    }
}
