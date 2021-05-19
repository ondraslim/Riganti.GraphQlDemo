using HotChocolate.Resolvers;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class AnimalType : ObjectType<Animal>
    {
        protected override void Configure(IObjectTypeDescriptor<Animal> descriptor)
        {
            descriptor
                .ImplementsNode()
                .IdField(x => x.Id)
                .ResolveNode((context, id) =>
                    context.DataLoader<AnimalByIdDataLoader>()
                        .LoadAsync(id, context.RequestAborted));

            descriptor
                .Field(x => x.Name)
                .Type<StringType>()
                .Description("The name of the Animal.");

            descriptor
                .Field(x => x.Species)
                .Type<StringType>()
                .UseUpperCase()
                .Description("The species of the Animal.");

            descriptor
                .Field(x => x.FarmId)
                .ID(nameof(Farm))
                .Description("The id of farm where the Animal lives");

            descriptor
                .Field(x => x.Farm)
                .Type<FarmType>()
                .ResolveWith<FarmResolvers>(r => r.GetFarmsByIdsAsync(default!, default!, default))
                .UseDbContext<AnimalFarmDbContext>()
                .Description("The farm where the Animal lives.");

        }
    }
}