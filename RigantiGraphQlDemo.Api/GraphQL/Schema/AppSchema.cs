using GraphQL;
using RigantiGraphQlDemo.Api.GraphQL.Mutations;
using RigantiGraphQlDemo.Api.GraphQL.Query;
using RigantiGraphQlDemo.Api.GraphQL.Subscriptions;

namespace RigantiGraphQlDemo.Api.GraphQL.Schema
{
    public class AppSchema : global::GraphQL.Types.Schema
    {
        public AppSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<AppQuery>();
            Mutation = resolver.Resolve<AnimalMutation>();
            Subscription = resolver.Resolve<AnimalSubscriptions>();
        }
    }
}