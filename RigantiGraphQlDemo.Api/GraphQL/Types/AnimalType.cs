using HotChocolate.Resolvers;
using HotChocolate.Types;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
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
                .Description("The name of the Animal.");

            descriptor
                .Field(x => x.Species)
                .UseUpperCase()
                .Description("The species of the Animal.");

            descriptor
                .Field(x => x.FarmId)
                .ID(nameof(Farm))
                .Description("The id of farm where the Animal lives");

            descriptor
                .Field(x => x.Farm)
                .UseProjection()
                .Description("The farm where the Animal lives.");

        }
    }
}