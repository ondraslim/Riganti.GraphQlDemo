using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public record AddAnimalInput(
            string Name,
            string Species,
            [ID(nameof(Farm))] int FarmId,
            string ClientMutationId)
        : InputBase(ClientMutationId);
}