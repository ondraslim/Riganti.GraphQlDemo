using GraphiQl;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RigantiGraphQlDemo.Api.GraphQL.Schema;
using RigantiGraphQlDemo.Api.Middleware;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.DataStore;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AnimalFarmDbContext>();
            services.AddScoped<IDataStore, DataStore>();

            services.AddScoped<IDependencyResolver>(_ => new FuncDependencyResolver(_.GetRequiredService));

            services.AddScoped<ISchema, AppSchema>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services
                .AddGraphQL(o => { o.ExposeExceptions = true; })
                .AddGraphTypes(ServiceLifetime.Scoped);

            services.AddSingleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>();
            services.AddSingleton<DataLoaderDocumentListener>();

            // If using Kestrel:
            services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });

            // If using IIS:
            services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // add graph ql
            app.UseGraphiQl("/GraphQL");

            app.UseMiddleware<GraphQlMiddleware>();
            app.UseGraphQL<ISchema>();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/GraphQL");
                    return Task.CompletedTask;
                });
            });
        }
    }
}