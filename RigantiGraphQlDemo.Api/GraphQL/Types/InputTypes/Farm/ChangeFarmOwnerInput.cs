using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public class ChangeFarmOwnerInput : InputBase
    {
        [ID(nameof(Dal.Entities.Farm))] 
        public int FarmId { get; }

        [ID(nameof(Dal.Entities.Person))] 
        public int NewOwnerId { get; }

        public ChangeFarmOwnerInput(string? clientMutationId, int newOwnerId, int farmId)
            : base(clientMutationId)
        {
            NewOwnerId = newOwnerId;
            FarmId = farmId;
        }
    }
}