﻿using RigantiGraphQlDemo.Api.Exceptions;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Animal
{
    public class AddAnimalPayload : AnimalPayloadBase
    {
        public AddAnimalPayload(Dal.Entities.Animal animal, string? clientMutationId)
            : base(animal, clientMutationId)
        {
        }

        public AddAnimalPayload(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}