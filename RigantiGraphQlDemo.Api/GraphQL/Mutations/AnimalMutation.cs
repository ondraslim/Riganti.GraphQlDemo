using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.Subscriptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{
    [ExtendObjectType(Name = "Mutation")]
    public class AnimalMutation
    {
        [UseApplicationDbContext]
        public async Task<AddAnimalPayload> AddAnimalAsync(
            AddAnimalInput input,
            [ScopedService] AnimalFarmDbContext db,
            [Service] ITopicEventSender eventSender,
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
            await eventSender.SendAsync(nameof(AnimalSubscriptions.OnAnimalCreatedAsync), animal.Id, token);

            return new AddAnimalPayload(animal, input.ClientMutationId);
        }

        [UseApplicationDbContext]
        public async Task<UpdateAnimalPayload> UpdateAnimalAsync(
            UpdateAnimalInput input,
            [ScopedService] AnimalFarmDbContext db,
            [Service] ITopicEventSender eventSender,
            CancellationToken token)
        {
            var animal = await db.Animals.FindAsync(input.Id);
            if (animal is null)
            {
                return new UpdateAnimalPayload(
                    new List<UserError> { new UserError("Animal not found.", "ANIMAL_NOT_FOUND") },
                    input.ClientMutationId);
            }

            var animalMoved = animal.FarmId != input.FarmId;
            animal.Name = input.Name;
            animal.Species = input.Species;
            animal.FarmId = input.FarmId;

            await db.SaveChangesAsync(token);

            if (animalMoved)
            {
                await eventSender.SendAsync(
                    $"{nameof(AnimalSubscriptions.OnAnimalMovedToFarm)}_{input.FarmId}", 
                    animal.Id,
                    token);
            }

            return new UpdateAnimalPayload(animal, input.ClientMutationId);
        }

        [UseApplicationDbContext]
        public async Task<DeleteAnimalPayload> DeleteAnimalAsync(
            DeleteAnimalInput input,
            [ScopedService] AnimalFarmDbContext db,
            CancellationToken token)
        {
            var animal = await db.Animals.FindAsync(input.Id);
            if (animal is null)
            {
                return new DeleteAnimalPayload(
                    new List<UserError> { new UserError("Animal not found.", "ANIMAL_NOT_FOUND") },
                    input.ClientMutationId);
            }

            db.Animals.Remove(animal);
            await db.SaveChangesAsync(token);

            return new DeleteAnimalPayload(animal, input.ClientMutationId);
        }


        [UseApplicationDbContext]
        public async Task<RenameAnimalPayload> RenameAnimalAsync(
            RenameAnimalInput input,
            [ScopedService] AnimalFarmDbContext db,
            CancellationToken token)
        {
            var animal = await db.Animals.FindAsync(input.Id);
            if (animal is null)
            {
                return new RenameAnimalPayload(
                    new List<UserError> { new UserError("Animal not found.", "ANIMAL_NOT_FOUND") },
                    input.ClientMutationId );
            }

            animal.Name = input.Name;
            await db.SaveChangesAsync(token);

            return new RenameAnimalPayload(animal, input.ClientMutationId);
        }
    }
}