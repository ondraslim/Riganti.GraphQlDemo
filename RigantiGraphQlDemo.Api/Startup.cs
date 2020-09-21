using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RigantiGraphQlDemo.Api.GraphQL.Mutations;
using RigantiGraphQlDemo.Api.GraphQL.Query;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal;

namespace RigantiGraphQlDemo.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AnimalFarmDbContext>(
                opt =>
                {
                    opt.UseSqlite("Data Source=animalFarm.db");
                    opt.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddDebug(); }));
                });
            
            services
                .AddGraphQL(sp => SchemaBuilder.New()
                    .AddServices(sp)
                    .AddQueryType<AppQuery>()
                    .AddMutationType<AnimalMutation>()
                    .AddType<AnimalType>()
                    .AddType<FarmType>()
                    .AddType<PersonType>()
                    .Create());

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

            app.UseRouting();
            
            app.UseGraphQL();

            // Add the GraphQL Playground UI to try out the GraphQL API at /
            // Add the GraphQL Voyager UI to let you navigate your GraphQL API as a spider graph at /voyager.
            app
                .UsePlayground("/")
                .UseVoyager("/voyager");
        }
    }
}