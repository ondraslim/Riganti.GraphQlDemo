using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public record RenameFarmInput(
            [ID(nameof(Dal.Entities.Farm))] int Id,
            string Name,
            string? ClientMutationId)
        : InputBase(ClientMutationId);
}