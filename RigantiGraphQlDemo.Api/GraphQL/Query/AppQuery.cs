using HotChocolate;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Linq;
using RigantiGraphQlDemo.Api.Extensions;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    public class AppQuery
    {
        [UseApplicationDbContext]
        public IQueryable<Person> GetPersons([Service] AnimalFarmDbContext db) =>
            db.Persons.Include(p => p.Farms).ThenInclude(f => f.Animals);

        [UseApplicationDbContext]
        public IQueryable<Farm> GetFarms([Service] AnimalFarmDbContext db) =>
            db.Farms.Include(p => p.Person).Include(f => f.Animals);
    }
}