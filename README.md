# GraphQL ASP.NET Core demo (5 - Client, Authentication, Authorization)

> This is just a brief demonstration of the GraphQL capabilities.

#### Client
Client can be easily created using `GraphQL.Client` and `GraphQL.Client.Serializer.Newtonsoft` libraries.
All you need to do is specify client options:

```csharp
private static readonly GraphQLHttpClientOptions GraphQlHttpClientOptions = new GraphQLHttpClientOptions
  {
      UseWebSocketForQueriesAndMutations = true,
      EndPoint = new Uri("https://localhost:44329/GraphQL")
  };
```

Create a client with the options:
```csharp
 using var client = new GraphQLHttpClient(GraphQlHttpClientOptions, new NewtonsoftJsonSerializer());
```

And then use client to create a request using `GraphQLHttpRequest` or subscribe:

```csharp
 var animalCreatedSubscription = CreateSubscription(client).Subscribe(
  response =>
  {
      if (response == null) return;

      Console.WriteLine($"new animal \"{response.Data.Name} - {response.Data.Species}\" in a farm of ID {response.Data.FarmId}");
  },
  exception => { Console.WriteLine($"subscription stream failed: {exception}"); },
  () => { Console.WriteLine("subscription stream completed"); }
  );
  
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
```

There are some generation tools, but I have no experience with them yet (e.g. https://github.com/Husqvik/GraphQlClientGenerator)


#### Authentication and Authorization

###### Sources:

* https://code-maze.com/consume-graphql-api-with-asp-net-core/
* https://github.com/graphql-dotnet/graphql-client#usage
* https://stackoverflow.com/questions/53537521/how-to-implement-authorization-using-graphql-net-at-resolver-function-level/59885020#59885020
* https://graphql-dotnet.github.io/docs/getting-started/malicious-queries/
* https://www.learmoreseekmore.com/2020/01/dotnet-core-graphql-api-authorization.html
* https://fullstackmark.com/post/22/build-an-authenticated-graphql-app-with-angular-aspnet-core-and-identityserver-part-1
