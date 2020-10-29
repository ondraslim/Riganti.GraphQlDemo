using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm.Base;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public class RenameFarmPayload : FarmPayloadBase
    {
        public RenameFarmPayload(Dal.Entities.Farm? farm, string? clientMutationId)
            : base(farm, clientMutationId)
        {
        }

        public RenameFarmPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}