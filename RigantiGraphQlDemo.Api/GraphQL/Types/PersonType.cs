using HotChocolate.Types;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class PersonType : ObjectType<Person>
    {
        protected override void Configure(IObjectTypeDescriptor<Person> descriptor)
        {
            descriptor
                .Field(x => x.Id)
                .Type<NonNullType<IdType>>()
                .Description("ID of the Person.");

            descriptor
                .Field(x => x.Name)
                .Type<StringType>()
                .Description("Name of the Person.");

            descriptor
                .Field(x => x.SecretPiggyBankLocation)
                .Type<StringType>()
                .Description("Secret location of person's piggy bank. (should not be available!)");

            descriptor
                .Field(x => x.Farms)
                .Type<ListType<FarmType>>()
                .Description("Farms owned by the Person.")
                .ResolveWith<FarmResolvers>(fr => fr.GetFarmsByPersonIdsAsync(default!, default!, default));
        }
    }
}