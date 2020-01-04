using Boxed.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public interface IPatchTenantCommand : IAsyncCommand<string, JsonPatchDocument<SaveTenant>>
    {
    }
}
