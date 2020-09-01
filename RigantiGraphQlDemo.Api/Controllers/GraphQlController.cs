using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using RigantiGraphQlDemo.Api.GraphQL;
using RigantiGraphQlDemo.Dal;
using System.Threading.Tasks;
using RigantiGraphQlDemo.Api.GraphQL.Query;
using RigantiGraphQlDemo.Api.GraphQL.Query.Model;

namespace RigantiGraphQlDemo.Api.Controllers
{

    [Route("graphql")]
    [ApiController]
    public class GraphQlController : Controller
    {
        private readonly AnimalFarmDbContext db;

        public GraphQlController(AnimalFarmDbContext db) => this.db = db;

        public async Task<IActionResult> Post([FromBody] GraphQlQuery query)
        {
            var inputs = query.Variables.ToInputs();

            var schema = new Schema
            {
                Query = new AppQuery(db)
            };

            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
            });

            if (result.Errors?.Count > 0)
            {
                return BadRequest();
            }

            return Ok(result);
        }
    }
}