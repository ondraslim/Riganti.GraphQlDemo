using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public class RenameFarmPayload : EntityPayloadBase<Dal.Entities.Farm>
    {
        public RenameFarmPayload(Dal.Entities.Farm? entity, string? clientMutationId)
            : base(entity, clientMutationId)
        {
        }

        public RenameFarmPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}