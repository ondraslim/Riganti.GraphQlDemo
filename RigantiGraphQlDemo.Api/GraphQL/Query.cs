using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Linq;

namespace RigantiGraphQlDemoBasic.GraphQL
{
    public class Query
    {
        [UseFirstOrDefault]
        [UseSelection]
        public IQueryable<Person> GetPersonById([Service] AnimalFarmDbContext context, int personId) =>
            context.Persons.Where(t => t.Id == personId);

        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Person> GetPersons([Service] AnimalFarmDbContext context) =>
            context.Persons;

        [UsePaging]
        [UseSelection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Farm> GetFarms([Service] AnimalFarmDbContext context) =>
            context.Farms;
    }
}