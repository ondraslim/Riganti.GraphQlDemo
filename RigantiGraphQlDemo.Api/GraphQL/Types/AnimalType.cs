using HotChocolate.Types;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class AnimalType : ObjectType<Animal>
    {
        protected override void Configure(IObjectTypeDescriptor<Animal> descriptor)
        {
            descriptor
                .Field(x => x.Id)
                .Type<NonNullType<IdType>>()
                .Description("The ID of the Animal.");

            descriptor
                .Field(x => x.Name)
                .Type<StringType>()
                .Description("The name of the Animal.");

            descriptor
                .Field(x => x.Species)
                .Type<StringType>()
                .Description("The species of the Animal.");

            descriptor
                .Field(x => x.FarmId)
                .Type<NonNullType<IdType>>()
                .Description("The id of farm where the Animal lives");

            descriptor
                .Field(x => x.Farm)
                .Type<FarmType>()
                .Description("The farm where the Animal lives.");

        }
    }
}