using HotChocolate;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    public class AppQuery
    {
        [UseApplicationDbContext]
        public Task<List<Person>> GetPersons([ScopedService] AnimalFarmDbContext db) =>
            db.Persons.ToListAsync();

        public Task<Person> GetPersonByIdAsync(
            int id,
            PersonByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        [UseApplicationDbContext]
        public Task<List<Farm>> GetFarms([ScopedService] AnimalFarmDbContext db) =>
            db.Farms.Include(p => p.Person).Include(f => f.Animals).ToListAsync();
    }
}