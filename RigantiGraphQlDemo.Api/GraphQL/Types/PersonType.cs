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
                .Description("Name of the Person.");

            descriptor
                .Field(x => x.SecretPiggyBankLocation)
                .Ignore()           // removes from schema, request for the field are not valid
                //  .Authorize()    // allow only specific users to access the field - policy/roles
                .Description("Secret location of person's piggy bank. (should not be available!)");

            descriptor
                .Field(x => x.Farms)
                .UsePaging<NonNullType<FarmType>>()
                .UseProjection()
                .UseFiltering()
                .UseSorting()
                .Description("Farms owned by the Person.");
        }
    }
}