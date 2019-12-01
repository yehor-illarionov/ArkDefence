using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Commands
{
    public class GetTenantCommand : IGetTenantCommand
    {
        public async Task<IActionResult> ExecuteAsync(string parameter, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
