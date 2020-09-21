using HotChocolate;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Linq;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    public class AppQuery
    {
        public IQueryable<Person> GetPersons([Service] AnimalFarmDbContext db) =>
            db.Persons.Include(p => p.Farms).ThenInclude(f => f.Animals);


        public IQueryable<Farm> GetFarms([Service] AnimalFarmDbContext db) =>
            db.Farms.Include(p => p.Person).Include(f => f.Animals);
    }
}