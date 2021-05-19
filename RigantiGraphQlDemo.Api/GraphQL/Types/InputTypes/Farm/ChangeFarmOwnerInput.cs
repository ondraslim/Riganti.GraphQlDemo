using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public record ChangeFarmOwnerInput(
            [ID(nameof(Dal.Entities.Farm))] int FarmId,
            [ID(nameof(Dal.Entities.Person))] int NewOwnerId,
            string? ClientMutationId)
        : InputBase(ClientMutationId);
}