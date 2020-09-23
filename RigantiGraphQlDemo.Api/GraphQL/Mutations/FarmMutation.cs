using HotChocolate;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm;
using RigantiGraphQlDemo.Dal;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Mutations
{

    [ExtendObjectType(Name = "Mutation")]
    public class FarmMutation
    {
        [UseApplicationDbContext]
        public async Task<RenameFarmPayload> RenameFarmAsync(
            RenameFarmInput input,
            [ScopedService] AnimalFarmDbContext db,
            CancellationToken token)
        {
            var farm = await db.Farms.FindAsync(input.Id);
            if (farm is null)
            {
                return new RenameFarmPayload(
                    new List<UserError> { new UserError("Farm not found.", "ANIMAL_NOT_FOUND") },
                    input.ClientMutationId);
            }

            farm.Name = input.Name;
            await db.SaveChangesAsync(token);

            return new RenameFarmPayload(farm, input.ClientMutationId);
        }


        [UseApplicationDbContext]
        public async Task<ChangeFarmOwnerPayload> ChangeFarmOwnerAsync(
            ChangeFarmOwnerInput input,
            [ScopedService] AnimalFarmDbContext db,
            CancellationToken token)
        {
            var farm = await db.Farms.FindAsync(input.FarmId);
            if (farm is null)
            {
                return new ChangeFarmOwnerPayload(
                    new List<UserError> { new UserError("Farm not found.", "ANIMAL_NOT_FOUND") },
                    input.ClientMutationId);
            }

            farm.PersonId = input.NewOwnerId;
            await db.SaveChangesAsync(token);

            return new ChangeFarmOwnerPayload(farm, input.ClientMutationId);
        }
    }
}