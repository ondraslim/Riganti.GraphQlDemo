using HotChocolate;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using HotChocolate.Types.Relay;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Animal;
using RigantiGraphQlDemo.Api.GraphQL.DataLoaders.Farm;
using RigantiGraphQlDemo.Dal.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Api.GraphQL.Subscriptions
{
    [ExtendObjectType(Name = "Subscription")]
    public class AnimalSubscriptions
    {
        /*
        * The [Topic] attribute can be put on the method or a parameter of the method and will infer the pub/sub-topic for this subscription.
        * The [Subscribe] attribute tells the schema builder that this resolver method needs to be hooked up to the pub/sub-system.
            This means that in the background, the resolver compiler will create a so-called subscribe resolver that handles subscribing to the pub/sub-system.
         * The [EventMessage] attribute marks the parameter where the execution engine shall inject the message payload of the pub/sub-system.
         */
        [Subscribe]
        [Topic]
        public Task<Animal> OnAnimalCreatedAsync(
            [EventMessage] int animalId,
            AnimalByIdDataLoader dataLoader,
            CancellationToken token) =>
            dataLoader.LoadAsync(animalId, token);

        [Subscribe(With = nameof(SubscribeToOnAnimalMovedToFarmAsync))]
        public AnimalMoved OnAnimalMovedToFarm(
            [ID(nameof(Farm))] int farmId,
            [EventMessage] int animalId,
            FarmByIdDataLoader dataLoader,
            CancellationToken token) =>
            new AnimalMoved(animalId, farmId);

        public async ValueTask<IAsyncEnumerable<int>> SubscribeToOnAnimalMovedToFarmAsync(
            int farmId,
            [Service] ITopicEventReceiver eventReceiver,
            CancellationToken token) =>
            await eventReceiver.SubscribeAsync<string, int>($"{nameof(OnAnimalMovedToFarm)}_{farmId}", token);
    }
}