using HotChocolate;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    public class AppQuery
    {
        [UseApplicationDbContext]
        public IQueryable<Person> GetPersons([Service] AnimalFarmDbContext db) =>
            db.Persons;

        public Task<Person> GetPersonByIdAsync(
            int id,
            PersonByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        [UseApplicationDbContext]
        public IQueryable<Farm> GetFarms([Service] AnimalFarmDbContext db) =>
            db.Farms.Include(p => p.Person).Include(f => f.Animals);
    }
}