using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class DeleteAnimalPayload : EntityPayloadBase<Dal.Entities.Animal>
    {
        public DeleteAnimalPayload(Dal.Entities.Animal? animal, string? clientMutationId) 
            : base(animal, clientMutationId)
        {
        }

        public DeleteAnimalPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}