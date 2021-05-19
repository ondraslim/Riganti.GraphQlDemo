using HotChocolate.Resolvers;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Person;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class PersonType : ObjectType<Person>
    {
        protected override void Configure(IObjectTypeDescriptor<Person> descriptor)
        {
            descriptor  
                .ImplementsNode()   // Relay
                .IdField(x => x.Id)
                .ResolveNode((context, id) =>
                    context.DataLoader<PersonByIdDataLoader>()
                        .LoadAsync(id, context.RequestAborted));

            descriptor
                .Field(x => x.Name)
                .Type<StringType>()
                .Description("Name of the Person.");

            descriptor
                .Field(x => x.SecretPiggyBankLocation)
                //.Ignore()
                .Type<StringType>()
                .Description("Secret location of person's piggy bank. (should not be available!)");

            descriptor
                .Field(x => x.Farms)
                .Type<ListType<FarmType>>()
                .UsePaging<NonNullType<FarmType>>()
                .UseFiltering()
                .UseSorting()
                .Description("Farms owned by the Person.")
                .ResolveWith<FarmResolvers>(fr => fr.GetFarmsByPersonIdsAsync(default!, default!, default))
                .UseDbContext<AnimalFarmDbContext>();
        }
    }
}