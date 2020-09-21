namespace RigantiGraphQlDemo.Api.GraphQL.InputTypes.Animal
{
    public class AddAnimalInput
    {
        public AddAnimalInput(string name, string species, int farmId, string clientMutationId)
        {
            Name = name;
            Species = species;
            FarmId = farmId;
            ClientMutationId = clientMutationId;
        }

        public string Name { get; }
        public string Species { get; }
        public int FarmId { get; }
        public string ClientMutationId { get; }
    }
}