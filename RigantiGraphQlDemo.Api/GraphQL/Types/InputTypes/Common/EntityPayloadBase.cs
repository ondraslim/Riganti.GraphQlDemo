using RigantiGraphQlDemo.Api.Exceptions;
using RigantiGraphQlDemo.Dal.Entities.Common;
using System.Collections.Generic;

namespace RigantiGraphQlDemo.Api.GraphQL.Types.InputTypes.Common
{
    public class EntityPayloadBase<TEntity> : PayloadBase
        where TEntity : EntityBase
    {
        public TEntity? Entity { get; }

        public EntityPayloadBase(TEntity? entity, string? clientMutationId)
            : base(clientMutationId)
        {
            Entity = entity;
        }

        public EntityPayloadBase(IReadOnlyList<UserError> errors, string? clientMutationId)
            : base(errors, clientMutationId)
        {
        }
    }
}