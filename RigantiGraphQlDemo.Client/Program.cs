using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using RigantiGraphQlDemo.Client.ResponseModels;
using System;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace RigantiGraphQlDemo.Client
{
    class Program
    {
        private static readonly GraphQLHttpClientOptions GraphQlHttpClientOptions = new GraphQLHttpClientOptions
        {
            UseWebSocketForQueriesAndMutations = true,
            EndPoint = new Uri("https://localhost:44329/GraphQL")
        };


        static async Task Main(string[] args)
        {
            Console.WriteLine("configuring client ...");
            using var client = new GraphQLHttpClient(GraphQlHttpClientOptions, new NewtonsoftJsonSerializer());

            Console.WriteLine("subscribing to animal stream ...");

            var errorSubscription = client.WebSocketReceiveErrors.Subscribe(e =>
            {
                if (e is WebSocketException we)
                    Console.WriteLine($"WebSocketException: {we.Message} " +
                                      $"(WebSocketError {we.WebSocketErrorCode}, " +
                                      $"ErrorCode {we.ErrorCode}, " +
                                      $"NativeErrorCode {we.NativeErrorCode}");
                else
                    Console.WriteLine($"Exception in websocket receive stream: {e}");
            });


            Console.WriteLine("subscribing to farm ID 1");
            var animalCreatedSubscription = CreateSubscription(client).Subscribe(
                response =>
                {
                    if (response == null) return;

                    Console.WriteLine($"new animal \"{response.Data.Name} - {response.Data.Species}\" in a farm of ID {response.Data.FarmId}");
                },
                exception => { Console.WriteLine($"subscription stream failed: {exception}"); },
                () => { Console.WriteLine("subscription stream completed"); }
                );
            await Task.Delay(200);


            Console.WriteLine("client setup completed");
            while (true)
            {
            }

            errorSubscription.Dispose();
            animalCreatedSubscription.Dispose();
        }


        private static IObservable<GraphQLResponse<AnimalCreatedModel>> CreateSubscription(GraphQLHttpClient client)
        {
            return client.CreateSubscriptionStream<AnimalCreatedModel>(
                new GraphQLRequest(@"
					subscription AnimalAddedSubscription {
                      animalCreatedInFarm(farm: 1) {
                        id
                        name
                      }
                    }"
                ));
        }
    }
}