using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Enexure.MicroBus;
using Enexure.MicroBus.Autofac;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApplication1.Data;
using WebApplication1.Hubs;
using Microsoft.OpenApi.Models;
using System.IO;
using WebApplication1.Exceptions;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Boxed.AspNetCore.Swagger;
using Boxed.AspNetCore.Swagger.OperationFilters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Boxed.AspNetCore.Swagger.SchemaFilters;
using WebApplication1.Handlers;

namespace WebApplication1
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostContext)
        {
            Configuration = configuration;
            _hostContext = hostContext;
        }

        private readonly IWebHostEnvironment _hostContext;
        public ILifetimeScope Container { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options=>
            {
                options.Filters.Add(new HttpResponseExceptionFilter());

            }).AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddRouting(
                options =>
                {
                    // All generated URL's should be lower-case.
                    options.LowercaseUrls = true;
                });
            services.AddDbContext<TenantDbContext>(options =>
              options.UseNpgsql(
                   Configuration.GetConnectionString("TenantConnection")));
            services.AddMultiTenant()
                .WithEFCoreStore<TenantDbContext, Tenant>()
                .WithRouteStrategy().WithFallbackStrategy("host");
            services.AddDbContext<NextAppContext>();
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(options =>
            {
                var assembly = typeof(Startup).Assembly;
                //var assemblyProduct = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
               // var assemblyDescription = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                options.DescribeAllParametersInCamelCase();
                options.EnableAnnotations();
                    // Add the XML comment file for this assembly, so its contents can be displayed.
                options.IncludeXmlCommentsIfExists(assembly);
                options.OperationFilter<CorrelationIdOperationFilter>();
                options.OperationFilter<ClaimsOperationFilter>();
                options.OperationFilter<ForbiddenResponseOperationFilter>();
                options.OperationFilter<UnauthorizedResponseOperationFilter>();
                // Show an example model for JsonPatchDocument<T>.
                options.SchemaFilter<JsonPatchDocumentSchemaFilter>();
                var info = new OpenApiInfo()
                    {
                        Title = "test",
                        Description = "test",
                        Version = "v1"
                    };
                options.SwaggerDoc("v1", info);
            });
            
            services.AddSignalR();

            var domain = $"https://{Configuration["Auth0:Domain"]}/";
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.Authority = domain;
                options.Audience = Configuration["Auth0:ApiIdentifier"];

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or 
                // Server-Sent Events request comes in.
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/hubs/")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            // Register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            // Change to use Name as the user identifier for SignalR
            // WARNING: This requires that the source of your JWT token 
            // ensures that the Name claim is unique!
            // If the Name claim isn't unique, users could receive messages 
            // intended for a different user!
            services.AddSingleton<IUserIdProvider, NameUserIdProvider>();
            //
            services.AddTransient<SystemControllerRepository>();
            services.AddTransient<TerminalRepository>();
            //
            services.AddDbContext<KeysContext>(options =>
             options.UseNpgsql(
                  Configuration.GetConnectionString("KeysConnection")));
            services.AddDataProtection()
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration() 
                {
                    EncryptionAlgorithm= EncryptionAlgorithm.AES_256_CBC,
                    ValidationAlgorithm= ValidationAlgorithm.HMACSHA256
                })
                .SetApplicationName("akrdefence_system_backend")
                .PersistKeysToDbContext<KeysContext>();
            services.AddHttpContextAccessor()
                    .AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddProjectCommands();
            services.AddProjectRepositories();
            services.AddProjectMappers();
            services.AddProjectServices();
        }

        // ConfigureContainer is where you can register things directly
        // with Autofac. This runs after ConfigureServices so the things
        // here will override registrations made in ConfigureServices.
        // Don't build the container; that gets done for you. If you
        // need a reference to the container, you need to use the
        // "Without ConfigureContainer" mechanism shown later.
        public void ConfigureContainer(ContainerBuilder builder)
        {
            //var busBuilder = new BusBuilder()
            //  .RegisterHandlers(Assembly.GetEntryAssembly());
            // // .RegisterGlobalHandler<TenantCreatedHandler>();
               
            //builder.RegisterMicroBus(busBuilder);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {  
               // app.UseExceptionHandler("/error");
            }
            app.UseExceptionHandler("/error");
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                
            });
            app.UseRouting();
            app.UseMultiTenant();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("host", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{__tenant__}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ControllerHub>("hubs/controllerhub");
            });
        }
    }
}
