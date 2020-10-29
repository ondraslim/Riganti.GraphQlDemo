using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class UpdateAnimalInput : InputBase
    {
        [ID(nameof(Dal.Entities.Animal))]
        public int Id { get; }
        public string Name { get; }
        public string Species { get; }

        [ID(nameof(Farm))]
        public int FarmId { get; }

        public UpdateAnimalInput(string? clientMutationId, int id, string name, string species, int farmId) 
            : base(clientMutationId)
        {
            Id = id;
            Name = name;
            Species = species;
            FarmId = farmId;
        }
    }
}