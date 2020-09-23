using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
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
    [ExtendObjectType(Name = "Query")]
    public class AnimalQueries
    {
        [UseApplicationDbContext]
        [UsePaging(SchemaType = typeof(NonNullType<AnimalType>))]
        public Task<List<Animal>> GetAnimals([ScopedService] AnimalFarmDbContext db) =>
            db.Animals.OrderBy(a => a.Name).ToListAsync();

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