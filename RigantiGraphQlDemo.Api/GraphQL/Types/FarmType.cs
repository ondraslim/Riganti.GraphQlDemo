using GraphQL.Types;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class FarmType : ObjectGraphType<Farm>
    {
        public FarmType()
        {
            Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
            Field(x => x.Name).Description("The name of the Farm.");
            Field(x => x.PersonId).Description("The id of the Farm's owner.");
            Field(x => x.Person, type: typeof(ListGraphType<PersonType>)).Description("The Farm's owner.");
            Field(x => x.Animals, type: typeof(ListGraphType<AnimalType>)).Description("Farm's animals");
        }
    }
}