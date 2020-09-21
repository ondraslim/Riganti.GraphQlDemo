using HotChocolate;
using RigantiGraphQlDemo.Api.GraphQL.InputTypes.Animal;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    public class AnimalMutation
    {
        public async Task<AddAnimalPayload> AddAnimalAsync(
            AddAnimalInput input,
            [Service] AnimalFarmDbContext db)
        {
            var animal = new Animal
            {
                Name = input.Name,
                Species = input.Species,
                FarmId = input.FarmId
            };

            await db.Animals.AddAsync(animal);
            await db.SaveChangesAsync();

            return new AddAnimalPayload(animal, input.ClientMutationId);
        }
    }
}