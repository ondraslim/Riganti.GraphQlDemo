using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.Extensions;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
using RigantiGraphQlDemo.Api.GraphQL.Resolvers;
using RigantiGraphQlDemo.Dal.Entities;

namespace RigantiGraphQlDemo.Api.GraphQL.Types
{
    public class AnimalType : ObjectType<Animal>
    {
        protected override void Configure(IObjectTypeDescriptor<Animal> descriptor)
        {
            descriptor
                .AsNode()
                .IdField(x => x.Id)
                .NodeResolver((context, id) =>
                    context.DataLoader<AnimalByIdDataLoader>().LoadAsync(id, context.RequestAborted));

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
                .Description("The farm where the Animal lives.");

        }
    }
}