using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class FarmType : ObjectType<Farm>
    {
        protected override void Configure(IObjectTypeDescriptor<Farm> descriptor)
        {
            descriptor
                .AsNode()
                .IdField(x => x.Id)
                .NodeResolver((context, id) =>
                    context.DataLoader<FarmByIdDataLoader>().LoadAsync(id, context.RequestAborted));

            descriptor
                .Field(x => x.Name)
                .Type<StringType>()
                .Description("The name of the Farm.");


            descriptor
                .Field(x => x.PersonId)
                .ID(nameof(Person))
                .Description("The id of the Farm's owner.");

            descriptor
                .Field(x => x.Person)
                .Type<PersonType>()
                .Description("Farm's owner.");

            descriptor
                .Field(x => x.Animals)
                .Type<ListType<AnimalType>>()
                .UsePaging<NonNullType<AnimalType>>()
                .UseFiltering()
                .UseSorting()
                .Description("Farm's animals.")
                .ResolveWith<AnimalResolvers>(ar => ar.GetAnimalsByFarmIdsAsync(default!, default!, default));
        }
    }
}