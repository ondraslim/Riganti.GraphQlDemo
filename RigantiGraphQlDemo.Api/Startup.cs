using GraphiQl;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using GraphQL.Types;
using GraphQL.Validation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RigantiGraphQlDemo.Api.Configuration;
using RigantiGraphQlDemo.Api.Configuration.Auth;
using RigantiGraphQlDemo.Api.GraphQL.Mutations;
using RigantiGraphQlDemo.Api.GraphQL.Schema;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.DataStore.Animal;
using RigantiGraphQlDemo.Dal.DataStore.Common;

namespace RigantiGraphQlDemo.Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AnimalFarmDbContext>(ServiceLifetime.Singleton);

            services.AddSingleton<IDataStore, DataStore>();
            services.AddSingleton<IAnimalDataStore, AnimalDataStore>();

            services.AddSingleton<IDependencyResolver>(_ => new FuncDependencyResolver(_.GetRequiredService));

            services.AddSingleton<ISchema, AppSchema>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o => { o.Cookie.Name = "graph-auth"; });

            services
                .AddTransient<IValidationRule, AuthorizationValidationRule>()
                .AddAuthorization(options =>
                {
                    options.AddPolicy(Policies.LoggedIn, p => p.RequireRole(Roles.UserRole, Roles.AdminRole));
                    options.AddPolicy(Policies.Admin, p => p.RequireRole(Roles.AdminRole));
                });

            services
                .AddGraphQL(o =>
                {
                    o.ExposeExceptions = true;
                    o.ComplexityConfiguration = GraphQlConfig.ComplexityConfiguration;
                    o.EnableMetrics = true;
                })
                .AddDataLoader()
                .AddGraphTypes()
                .AddUserContextBuilder(o => o.User = GraphQlConfig.FakedUser)
                .AddWebSockets();

            services.AddSingleton<IMutation, LoginMutation>();
            services.AddSingleton<IMutation, AnimalMutation>();


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

            // Use the GraphQL subscriptions in the specified schema and make them available.
            app.UseWebSockets();

            app.UseAuthentication()
                .UseAuthorization()
                .UseGraphQL<ISchema>();

            // add graph ql
            app.UseGraphiQl("/GraphiQL");

            app.UseGraphQLWebSockets<ISchema>();


            // Add the GraphQL Playground UI to try out the GraphQL API at /
            // Add the GraphQL Voyager UI to let you navigate your GraphQL API as a spider graph at /voyager.
            app
                .UseGraphQLPlayground(new GraphQLPlaygroundOptions { Path = "/" })
                .UseGraphQLVoyager(new GraphQLVoyagerOptions { Path = "/voyager" });
        }
    }
}