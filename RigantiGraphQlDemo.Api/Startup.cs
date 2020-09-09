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
using RigantiGraphQlDemo.Dal.DataStore.Animal;
using RigantiGraphQlDemo.Dal.DataStore.Common;
using System.Threading.Tasks;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;

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
            services.AddScoped<IAnimalDataStore, AnimalDataStore>();

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

            // Use the GraphQL subscriptions in the specified schema and make them available.

            app.UseWebSockets();
            app.UseGraphQLWebSockets<ISchema>();

            //app.UseMiddleware<GraphQlMiddleware>();
            app.UseGraphQL<ISchema>();

            // Add the GraphQL Playground UI to try out the GraphQL API at /
            // Add the GraphQL Voyager UI to let you navigate your GraphQL API as a spider graph at /voyager.
            app
                .UseGraphQLPlayground(new GraphQLPlaygroundOptions{ Path = "/" })   
                .UseGraphQLVoyager(new GraphQLVoyagerOptions{ Path = "/voyager" });
            

            app.UseRouting();
        }
    }
}