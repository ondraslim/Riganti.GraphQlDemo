using Microsoft.Extensions.DependencyInjection;
using RigantiGraphQlDemo.Api.GraphQL.Mutations;
using RigantiGraphQlDemo.Api.GraphQL.Query;
using RigantiGraphQlDemo.Api.GraphQL.Subscriptions;
using System;

namespace RigantiGraphQlDemo.Api.GraphQL.Schema
{
    public class AppSchema : global::GraphQL.Types.Schema
    {
        public AppSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<AppQuery>();
            Mutation = provider.GetRequiredService<RootMutation>();
            Subscription = provider.GetRequiredService<AnimalSubscriptions>();
        }
    }
}