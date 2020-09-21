using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.InputTypes.Animal
{
    public class AddAnimalPayload
    {
        public AddAnimalPayload(Dal.Entities.Animal animal, string clientMutationId)
        {
            Id = animal.Id;
            Name = animal.Name;
            Species = animal.Species;
            FarmId = animal.FarmId;
            Farm = animal.Farm;
            ClientMutationId = clientMutationId;
        }

        public int Id { get; set; }
        public string Name { get; }
        public string Species { get; }
        public int FarmId { get; }
        public Farm Farm { get; }
        public string ClientMutationId { get; }
    }
}