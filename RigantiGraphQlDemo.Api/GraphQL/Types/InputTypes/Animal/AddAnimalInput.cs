using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class AddAnimalInput : InputBase
    {
        public string Name { get; }
        public string Species { get; }
        public int FarmId { get; }


        public AddAnimalInput(string name, string species, int farmId, string clientMutationId)
            : base(clientMutationId)
        {
            Name = name;
            Species = species;
            FarmId = farmId;
        }
    }
}