using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using RigantiGraphQlDemo.Api.GraphQL.Query.Model;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.Controllers
{
    [Route("[controller]")]
    public class GraphQlController : Controller
    {
        private readonly ISchema schema;
        private readonly IDocumentExecuter documentExecutor;

        public GraphQlController(ISchema schema, IDocumentExecuter documentExecutor)
        {
            this.schema = schema;
            this.documentExecutor = documentExecutor;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQlQuery query)
        {
            var inputs = query.Variables.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = schema,
                Query = query.Query,
                OperationName = query.OperationName,
                Inputs = inputs
            };

            var result = await documentExecutor.ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}