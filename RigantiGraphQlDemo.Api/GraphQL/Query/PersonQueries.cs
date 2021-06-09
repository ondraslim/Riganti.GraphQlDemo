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
        // [Authorize("POLICY/ROLE")]
        [UsePaging(typeof(NonNullType<PersonType>))]
        [UseFiltering]
        [UseSorting]
        public IOrderedQueryable<Person> GetPersons([ScopedService] AnimalFarmDbContext db) =>
            // anything that returns IQueryable
            db.Persons.OrderBy(p => p.Name);

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