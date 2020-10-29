using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class DeleteAnimalInput : InputBase
    {
        [ID(nameof(Dal.Entities.Animal))]
        public int Id { get; }

        public DeleteAnimalInput(string? clientMutationId, int id) : base(clientMutationId)
        {
            Id = id;
        }
    }
}