using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public record RenameAnimalInput(
            [ID(nameof(Dal.Entities.Animal))] int Id,
            string Name,
            string? ClientMutationId)
        : InputBase(ClientMutationId);
}