using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RigantiGraphQlDemo.Api.GraphQL.Authentication;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person;
using RigantiGraphQlDemo.Api.GraphQL.Mutations;
using RigantiGraphQlDemo.Api.GraphQL.Query;
using RigantiGraphQlDemo.Api.GraphQL.Subscriptions;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal;
using Serilog;

namespace RigantiGraphQlDemo.Api
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // db
            services.AddPooledDbContextFactory<AnimalFarmDbContext>(
                opt =>
                {
                    opt.UseSqlite("Data Source=animalFarm.db");
                    opt.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddDebug(); }));
                });

            // schema
            services
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType(q => q.Name("Query"))
                    .AddTypeExtension<PersonQueries>()
                    .AddTypeExtension<FarmQueries>()
                    .AddTypeExtension<AnimalQueries>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<AnimalMutation>()
                    .AddTypeExtension<FarmMutation>()
                .AddSubscriptionType(d => d.Name("Subscription"))
                    .AddTypeExtension<AnimalSubscriptions>()
                .AddType<AnimalType>()
                .AddType<FarmType>()
                .AddType<PersonType>()
                .AddFiltering()
                .AddSorting()
                .EnableRelaySupport()
                .AddInMemorySubscriptions()     // Subscriptions
                .AddDataLoader<PersonByIdDataLoader>()      // DataLoaders
                .AddDataLoader<FarmByIdDataLoader>()
                .AddDataLoader<FarmsByPersonIdDataLoader>()
                .AddDataLoader<AnimalByFarmIdDataLoader>()
                .AddDataLoader<AnimalByFarmIdDataLoader>();

            // auth
            services.AddAuthorization();
            services.AddAuthentication("GraphQL")
                .AddScheme<DummyAuthOptions, DummyAuth>("GraphQL", opt => { });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // subscriptions, graphql
            app.UseWebSockets();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapGraphQL("/"));
        }
    }
}