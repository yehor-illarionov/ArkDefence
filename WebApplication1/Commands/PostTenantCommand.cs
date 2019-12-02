using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.ViewModels;

namespace WebApplication1.Commands
{
    public class PostTenantCommand : IPostTenantCommand
    {
        public async Task<IActionResult> ExecuteAsync(SaveTenant parameter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
