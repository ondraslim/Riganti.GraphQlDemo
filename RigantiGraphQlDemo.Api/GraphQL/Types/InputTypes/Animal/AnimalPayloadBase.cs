using System.Collections.Generic;
using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class AnimalPayloadBase : PayloadBase
    {
        public Dal.Entities.Animal? Animal { get; }

        public AnimalPayloadBase(Dal.Entities.Animal? animal, string? clientMutationId)
            : base(clientMutationId)
        {
            Animal = animal;
        }

        public AnimalPayloadBase(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}