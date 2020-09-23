using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class RenameAnimalPayload : EntityPayloadBase<Dal.Entities.Animal>
    {
        public RenameAnimalPayload(Dal.Entities.Animal animal, string? clientMutationId)
            : base(animal, clientMutationId)
        {
        }

        public RenameAnimalPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}