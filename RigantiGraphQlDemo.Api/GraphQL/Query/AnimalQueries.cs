using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
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
    public class AnimalQueries
    {
        [UseApplicationDbContext]
        [UsePaging(typeof(NonNullType<AnimalType>))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Animal> GetAnimals([ScopedService] AnimalFarmDbContext db) => db.Animals;

        public Task<Animal> GetAnimalAsync(
            [ID(nameof(Animal))] int id,
            AnimalByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public Task<IReadOnlyList<Animal>> GetAnimalsAsync(
            [ID(nameof(Animal))] int[] ids,
            AnimalByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}