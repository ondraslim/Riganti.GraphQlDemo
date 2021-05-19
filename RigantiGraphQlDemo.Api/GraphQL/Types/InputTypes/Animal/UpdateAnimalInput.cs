using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public record UpdateAnimalInput(
            [ID(nameof(Dal.Entities.Animal))] int Id, 
            [ID(nameof(Dal.Entities.Farm))] int FarmId, 
            string Name,
            string Species,
            string? ClientMutationId)
        : InputBase(ClientMutationId);
}