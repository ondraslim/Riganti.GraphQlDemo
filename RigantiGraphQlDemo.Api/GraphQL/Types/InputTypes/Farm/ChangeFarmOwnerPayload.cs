using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm.Base;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public class ChangeFarmOwnerPayload : FarmPayloadBase
    {
        public ChangeFarmOwnerPayload(Dal.Entities.Farm? farm, string? clientMutationId) 
            : base(farm, clientMutationId)
        {
        }

        public ChangeFarmOwnerPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}