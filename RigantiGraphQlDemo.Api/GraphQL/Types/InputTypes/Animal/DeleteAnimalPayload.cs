using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class DeleteAnimalPayload : AnimalPayloadBase
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