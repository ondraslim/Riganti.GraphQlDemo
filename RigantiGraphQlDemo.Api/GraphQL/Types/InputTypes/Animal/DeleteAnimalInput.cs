using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public record DeleteAnimalInput(
            [ID(nameof(Dal.Entities.Animal))] int Id, 
            string? ClientMutationId)
        : InputBase(ClientMutationId);
}