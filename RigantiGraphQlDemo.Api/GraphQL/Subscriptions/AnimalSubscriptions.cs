using GraphQL.Resolvers;
using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.Types.AnimalTypes;
using RigantiGraphQlDemo.Dal.DataStore.Animal;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace RigantiGraphQlDemo.Api.GraphQL.Subscriptions
{
    public sealed class AnimalSubscriptions : ObjectGraphType<object>
    {
        public AnimalSubscriptions(IAnimalDataStore animalDataStore)
        {
            Name = "Animal Subscription";
            Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets.";

            AddField(
                new EventStreamFieldType
                {
                    Name = "animalCreatedInFarm",
                    // I want to know, when my neighbor (opponent) gets a new animal
                    Description = "Subscribe to get updates when new animal is created in specific farms.",
                    Arguments = new QueryArguments(
                        new QueryArgument<ListGraphType<IdGraphType>>
                        {
                            Name = "homeFarms"
                        }),
                    Type = typeof(AnimalCreatedEvent),
                    Resolver = new FuncFieldResolver<Animal>(context =>
                        context.Source as Animal),
                    Subscriber = new EventStreamResolver<Animal>(context =>
                    {
                        var homeFarms = context.GetArgument<List<int>>("homeFarms");
                        return animalDataStore
                            .WhenAnimalCreated
                            .Where(x => homeFarms is null || homeFarms.Contains(x.FarmId));
                    }),
                });
        }
    }
}