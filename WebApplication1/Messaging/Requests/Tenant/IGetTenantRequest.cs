using System;
using Boxed.AspNetCore;

namespace WebApplication1.Commands
{
    public interface IGetTenantRequest : IAsyncCommand<string>
    {
    }
}
