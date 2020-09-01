using GraphQL.Types;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.InputTypes
{
    public class AnimalInputType : InputObjectGraphType
    {
        public AnimalInputType()
        {
            Field<NonNullGraphType<StringGraphType>>(nameof(Animal.Name));
            Field<StringGraphType>(nameof(Animal.Species));
            Field<IdGraphType>(nameof(Animal.FarmId));
        }
    }
}