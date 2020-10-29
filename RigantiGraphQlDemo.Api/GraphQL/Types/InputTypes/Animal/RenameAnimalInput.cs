using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class RenameAnimalInput : InputBase
    {
        [ID(nameof(Dal.Entities.Animal))]
        public int Id { get; }
        public string Name { get; }

        public RenameAnimalInput(string? clientMutationId, string name, int id)
            : base(clientMutationId)
        {
            Name = name;
            Id = id;
        }
    }
}