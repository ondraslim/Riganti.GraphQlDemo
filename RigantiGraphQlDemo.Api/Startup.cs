using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person;
using RigantiGraphQlDemo.Api.GraphQL.Mutations;
using RigantiGraphQlDemo.Api.GraphQL.Query;
using RigantiGraphQlDemo.Api.GraphQL.Subscriptions;
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

            services.AddDataLoader<PersonByIdDataLoader>();
            
            services.AddDataLoader<FarmByIdDataLoader>();
            services.AddDataLoader<FarmsByPersonIdDataLoader>();

            services.AddDataLoader<AnimalByFarmIdDataLoader>();
            services.AddDataLoader<AnimalByFarmIdDataLoader>();

            services.AddInMemorySubscriptions();

            services
                .AddGraphQL(sp => SchemaBuilder.New()
                    .AddServices(sp)
                    .AddQueryType(q => q.Name("Query"))
                        .AddType<PersonQueries>()
                        .AddType<FarmQueries>()
                        .AddType<AnimalQueries>()
                    .AddMutationType(d => d.Name("Mutation"))
                        .AddType<AnimalMutation>()
                        .AddType<FarmMutation>()
                    .AddSubscriptionType(d => d.Name("Subscription"))
                        .AddType<AnimalSubscriptions>()
                    .AddType<AnimalType>()
                    .AddType<FarmType>()
                    .AddType<PersonType>()
                    .EnableRelaySupport()
                    .Create());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
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