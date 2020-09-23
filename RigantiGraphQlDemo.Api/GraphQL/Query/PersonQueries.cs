using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person;
using RigantiGraphQlDemo.Api.GraphQL.Types;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Query
{
    [ExtendObjectType(Name = "Query")]
    public class PersonQueries
    {
        [UseApplicationDbContext]
        [UsePaging(SchemaType = typeof(NonNullType<PersonType>))]
        public Task<List<Person>> GetPersons([ScopedService] AnimalFarmDbContext db) =>
            db.Persons.OrderBy(p => p.Name).ToListAsync();

        public Task<Person> GetPersonAsync(
            [ID(nameof(Person))] int id,
            PersonByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public Task<IReadOnlyList<Person>> GetPersonsAsync(
            [ID(nameof(Person))] int[] ids,
            PersonByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}