using HotChocolate.Types;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class FarmType : ObjectType<Farm>
    {
        protected override void Configure(IObjectTypeDescriptor<Farm> descriptor)
        {
            descriptor
                .Field(x => x.Id)
                .Type<NonNullType<IdType>>()
                .Description("The ID of the Farm.");

            descriptor
                .Field(x => x.Name)
                .Type<StringType>()
                .Description("The name of the Farm.");


            descriptor
                .Field(x => x.PersonId)
                .Type<NonNullType<IdType>>()
                .Description("The id of the Farm's owner.");

            descriptor
                .Field(x => x.Person)
                .Type<PersonType>()
                .Description("Farm's owner.");

            descriptor
                .Field(x => x.Animals)
                .Type<ListType<AnimalType>>()
                .Description("Farm's animals.")
                .ResolveWith<AnimalResolvers>(ar => ar.GetAnimalsByFarmIdsAsync(default!, default!, default));
        }
    }
}