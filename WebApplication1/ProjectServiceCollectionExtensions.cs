using System;
using Boxed.Mapping;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Commands;
using WebApplication1.Mappers;
using WebApplication1.Repositories;

namespace WebApplication1
{
    public static class ProjectServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectCommands(this IServiceCollection services)=>
            services
                .AddTransient<IDeleteTenantCommand, DeleteTenantCommmand>()
                .AddTransient<IGetTenantCommand,GetTenantCommand>();

        public static IServiceCollection AddProjectRepositories(this IServiceCollection services)=>
            services.AddSingleton<ITenantRepository,TenantRepository>();

        public static IServiceCollection AddProjectMappers(this IServiceCollection services) =>
            services.AddTransient<IMapper<Data.Tenant, ViewModels.Tenant>, TenantToTenantMapper>();
    }
}
