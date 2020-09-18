using GraphQL.Types;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation(IEnumerable<IMutation> mutations)
        {
            Name = "RootMutation";
            foreach (var marker in mutations)
            {
                var q = marker as ObjectGraphType;
                foreach (var f in q.Fields)
                {
                    AddField(f);
                }
            }
        }
    }
}