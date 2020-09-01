using GraphQL.Types;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class AnimalType : ObjectGraphType<Animal>
    {
        public AnimalType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Animal.");
            Field(x => x.Name).Description("The name of the Animal.");
            Field(x => x.Species).Description("The species of the Animal.");
            Field(x => x.FarmId).Description("The id of farm where the Animal lives.");
            Field(x => x.Farm, type: typeof(ListGraphType<FarmType>)).Description("The farm where the Animal lives.");
        }
    }
}