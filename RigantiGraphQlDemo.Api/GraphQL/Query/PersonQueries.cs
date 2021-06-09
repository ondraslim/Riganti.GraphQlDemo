using HotChocolate;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
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
    [ExtendObjectType("Query")]
    public class PersonQueries
    {
        [UseApplicationDbContext]
        [Authorize]
        [UsePaging(typeof(NonNullType<PersonType>))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        // anything that returns IQueryable
        public IQueryable<Person> GetPersons([ScopedService] AnimalFarmDbContext db) => db.Persons;

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