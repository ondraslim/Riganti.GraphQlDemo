using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public class RenameFarmInput : InputBase
    {
        [ID(nameof(Dal.Entities.Farm))]
        public int Id { get; }
        public string Name { get; }

        public RenameFarmInput(string? clientMutationId, string name, int id)
            : base(clientMutationId)
        {
            Name = name;
            Id = id;
        }
    }
}