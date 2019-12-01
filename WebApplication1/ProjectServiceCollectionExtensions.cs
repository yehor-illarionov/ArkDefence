using System;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Commands;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services)=>
            services.AddSingleton<IDeleteTenantCommand, DeleteTenantCommmand>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services)=>
            services.AddSingleton<ITenantRepository,TenantRepository>();
    }
}
