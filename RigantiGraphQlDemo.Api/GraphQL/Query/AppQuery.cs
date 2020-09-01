using GraphQL.Types;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal;
using System.Linq;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    public class AppQuery : ObjectGraphType
    {
        public AppQuery(AnimalFarmDbContext dbContext)
        {
            Field<PersonType>(
                "Person",
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> {Name = "id", Description = "The ID of the Person."}),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return dbContext.Persons.Include(p => p.Farms).ThenInclude(pf => pf.Animals).FirstOrDefault(p => p.Id == id);
                }
            );

            Field<ListGraphType<PersonType>>(
                "Persons",
                resolve: context =>
                {
                    return dbContext.Persons.Include(p => p.Farms).ThenInclude(pf => pf.Animals);
                }
            );

            Field<ListGraphType<FarmType>>(
                "Farms",
                resolve: context =>
                {
                    return dbContext.Farms.Include(f => f.Animals).Include(f => f.Person);
                }
            );
        }
    }
}