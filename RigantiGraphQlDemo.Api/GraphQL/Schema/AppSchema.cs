using GraphQL;
using RigantiGraphQlDemo.Api.GraphQL.Query;

namespace RigantiGraphQlDemo.Api.GraphQL.Schema
{
    public class AppSchema : global::GraphQL.Types.Schema
    {
        public AppSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<AppQuery>();
        }
    }
}