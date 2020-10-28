using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using HotChocolate.AspNetCore.Voyager;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // db
            services.AddDbContextPool<AnimalFarmDbContext>(
                opt =>
                {
                    opt.UseSqlite("Data Source=animalFarm.db");
                    opt.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddDebug(); }));
                });

            #region GraphQL

            // DataLoaders
            services.AddDataLoaderRegistry();
            services.AddDataLoader<PersonByIdDataLoader>();
            
            services.AddDataLoader<FarmByIdDataLoader>();
            services.AddDataLoader<FarmsByPersonIdDataLoader>();

            services.AddDataLoader<AnimalByFarmIdDataLoader>();
            services.AddDataLoader<AnimalByFarmIdDataLoader>();

            // Subscriptions
            services.AddInMemorySubscriptions();

            // schema
            services
                .AddGraphQL(sp => SchemaBuilder.New()
                    .AddAuthorizeDirectiveType()
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
            #endregion

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
            app.UseAuthentication();

            app.UseGraphQL();

            // Add the GraphQL Playground UI to try out the GraphQL API at /
            // Add the GraphQL Voyager UI to let you navigate your GraphQL API as a spider graph at /voyager.
            app
                .UsePlayground(new PathString("/"))
                .UseVoyager(new PathString("/voyager"));
        }
    }
}