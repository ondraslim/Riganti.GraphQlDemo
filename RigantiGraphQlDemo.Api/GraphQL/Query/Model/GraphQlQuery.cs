using Newtonsoft.Json.Linq;

namespace RigantiGraphQlDemo.Api.GraphQL.Query.Model
{
    public class GraphQlQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}