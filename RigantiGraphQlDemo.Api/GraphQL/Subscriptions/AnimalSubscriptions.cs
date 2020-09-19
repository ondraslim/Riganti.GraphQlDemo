using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Subscription;
using GraphQL.Types;
using RigantiGraphQlDemo.Api.GraphQL.Types.AnimalTypes;
using RigantiGraphQlDemo.Dal.DataStore.Animal;
using RigantiGraphQlDemo.Dal.Entities;
using System;
using System.Reactive.Linq;

namespace RigantiGraphQlDemo.Api.GraphQL.Subscriptions
{
    public class AnimalSubscriptions : ObjectGraphType
    {
        public AnimalSubscriptions(IAnimalDataStore animalDataStore)
        {
            Name = "AnimalSubscription";
            Description = "The subscription type, represents all updates can be pushed to the client in real time over web sockets.";

            AddField(
                new EventStreamFieldType
                {
                    Name = "animalCreatedInFarm",
                    // I want to know, when my neighbor (opponent) gets a new animal
                    Description = "Subscribe to get updates when new animal is created in specific farms.",
                    Arguments = new QueryArguments(
                        new QueryArgument<IdGraphType>
                        {
                            Name = "farm"
                        }),
                    Type = typeof(AnimalCreatedEvent),
                    Resolver = new FuncFieldResolver<Animal>(ResolveAnimal),
                    Subscriber = new EventStreamResolver<Animal>(ctx => Subscribe(ctx, animalDataStore)),
                });
        }

        private Animal ResolveAnimal(IResolveFieldContext context)
        {
            return context.Source as Animal;
        }

        private IObservable<Animal> Subscribe(IResolveEventStreamContext context, IAnimalDataStore animalDataStore)
        {
            var farm = context.GetArgument<int>("farm");

            return animalDataStore
                .AnimalCreated
                .Where(x => farm == x.FarmId);
        }
    }
}