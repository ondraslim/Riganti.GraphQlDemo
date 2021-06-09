using HotChocolate.Resolvers;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class FarmType : ObjectType<Farm>
    {
        protected override void Configure(IObjectTypeDescriptor<Farm> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(x => x.Id)
                .ResolveNode((context, id) => 
                    context.DataLoader<FarmByIdDataLoader>()
                        .LoadAsync(id, context.RequestAborted));

            descriptor
                .Field(x => x.Name)
                .Description("The name of the Farm.");


            descriptor
                .Field(x => x.PersonId)
                .ID(nameof(Person))
                .Description("The id of the Farm's owner.");

            descriptor
                .Field(x => x.Person)
                .Description("Farm's owner.");

            descriptor
                .Field(x => x.Animals)
                .ResolveWith<AnimalResolvers>(ar => ar.GetAnimalsByFarmIdsAsync(default!, default!, default))
                .UseDbContext<AnimalFarmDbContext>()
                .UsePaging<NonNullType<AnimalType>>()
                .UseFiltering()
                .UseSorting()
                .Description("Farm's animals.");
        }
    }
}