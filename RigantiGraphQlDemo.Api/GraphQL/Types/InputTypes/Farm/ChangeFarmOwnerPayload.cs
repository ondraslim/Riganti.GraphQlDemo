using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm
{
    public class ChangeFarmOwnerPayload : EntityPayloadBase<Dal.Entities.Farm>
    {
        public ChangeFarmOwnerPayload(Dal.Entities.Farm? entity, string? clientMutationId) 
            : base(entity, clientMutationId)
        {
        }

        public ChangeFarmOwnerPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}