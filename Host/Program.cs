using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.InProcess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /*
                https://github.com/aspnet/AspNetCore/issues/4206#issuecomment-445612167
                CurrentDirectoryHelpers exists in: \framework\src\Volo.Abp.AspNetCore.Mvc\Microsoft\AspNetCore\InProcess\CurrentDirectoryHelpers.cs
                Will remove CurrentDirectoryHelpers.cs when upgrade to ASP.NET Core 3.0.
            */
            CurrentDirectoryHelpers.SetCurrentDirectory();

            BuildWebHostInternal(args).Run();
        }

        public static IWebHost BuildWebHostInternal(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIIS()
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
