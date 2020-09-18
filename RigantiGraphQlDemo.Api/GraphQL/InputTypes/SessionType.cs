using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.Authentication;

namespace RigantiGraphQlDemo.Api.GraphQL.InputTypes
{
    public class SessionType : ObjectGraphType<Session>
    {
        public SessionType()
        {
            Field(t => t.IsLoggedIn);
            Field(t => t.IsAdmin);
        }
    }
}