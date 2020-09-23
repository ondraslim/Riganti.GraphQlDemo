using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Farm.Base
{
    public class FarmPayloadBase : PayloadBase
    {
        public Dal.Entities.Farm? Farm { get; }

        public FarmPayloadBase(Dal.Entities.Farm? farm, string? clientMutationId)
            : base(clientMutationId)
        {
            Farm = farm;
        }

        public FarmPayloadBase(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}