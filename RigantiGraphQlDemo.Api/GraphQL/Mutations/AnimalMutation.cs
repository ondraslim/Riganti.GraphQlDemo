using HotChocolate;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Threading;
using System.Threading.Tasks;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class AnimalMutation
    {
        [UseApplicationDbContext]
        public async Task<AddAnimalPayload> AddAnimalAsync(
            AddAnimalInput input,
            [ScopedService] AnimalFarmDbContext db,
            CancellationToken token)
        {
            var animal = new Animal
            {
                Name = input.Name,
                Species = input.Species,
                FarmId = input.FarmId
            };

            await db.Animals.AddAsync(animal, token);
            await db.SaveChangesAsync(token);

            return new AddAnimalPayload(animal, input.ClientMutationId);
        }
    }
}