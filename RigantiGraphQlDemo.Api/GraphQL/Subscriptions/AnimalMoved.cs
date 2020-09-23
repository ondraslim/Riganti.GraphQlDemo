using HotChocolate;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Subscriptions
{
    public class AnimalMoved
    {
        [ID(nameof(Animal))]
        public int AnimalId { get; }
        
        [ID(nameof(Farm))]
        public int FarmId { get; }

        public AnimalMoved(int animalId, int farmId)
        {
            AnimalId = animalId;
            FarmId = farmId;
        }
        
        public Task<Animal> GetAnimalAsync(
            AnimalByIdDataLoader dataLoader,
            CancellationToken token) =>
            dataLoader.LoadAsync(AnimalId, token);


        public Task<Farm> GetAnimalAsync(
            FarmByIdDataLoader dataLoader,
            CancellationToken token) =>
            dataLoader.LoadAsync(FarmId, token);
    }
}