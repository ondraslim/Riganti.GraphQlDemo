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

> There are some generation tools, but I have no experience with them yet (e.g. https://github.com/Husqvik/GraphQlClientGenerator)

#### Authentication and Authorization

GraphQL in dotnet provides **authorization** options through policies. We can define policies nad roles:

```csharp
public static class Policies
{
    public static string LoggedIn { get; } = "LoggedIn";
    public static string Admin { get; } = "Admin";
}

public static class Roles
{
    public static string UserRole { get; } = "User";
    public static string AdminRole { get; } = "Admin";
}
```

Register authorization and validation in our IoC container:

```csharp
services
      .AddTransient<IValidationRule, AuthorizationValidationRule>()
      .AddAuthorization(options =>
      {
          options.AddPolicy(Policies.LoggedIn, p => p.RequireRole(Roles.UserRole, Roles.AdminRole));
          options.AddPolicy(Policies.Admin, p => p.RequireRole(Roles.AdminRole));
      });
```

Add it to the pipeline:
                

And then authorize access to specific fields of a GraphQL type with specific policy. For instance we might want to allow to access the list of person's farms only to logged users and we might finally want to expose the `Person.SecretPiggyBankLocation` property to admins only:

```csharp
public class PersonType : ObjectGraphType<Person>
{
    public PersonType(IDataStore dataStore, IDataLoaderContextAccessor accessor)
    {
        Field(x => x.Id, type: typeof(IdGraphType)).Description("The ID of the Farm.");
        Field(x => x.Name).Description("The name of the Farm.");
        Field<ListGraphType<FarmType>, IEnumerable<Farm>>()
            .Name("Farms")
            .Description("The farms of the Person.")
            .AuthorizeWith(Policies.LoggedIn)
            .ResolveAsync(ctx =>
                {
                    var farmLoader =
                        accessor.Context.GetOrAddCollectionBatchLoader<int, Farm>(
                            "GetFarmsByPersonId",
                            dataStore.GetFarmsByPersonIdDataLoaderAsync);

                    return farmLoader.LoadAsync(ctx.Source.Id);
                }
            );

        Field(x => x.SecretPiggyBankLocation)
            .Description("The secret location of person's piggy bank. (should not be available!)")
            .AuthorizeWith(Policies.Admin);
    }
}
```

For the sake of simplicity, the **Authentication** is very simplified and we fake having a logged in user:

```csharp
public static ClaimsPrincipal FakedUser => new ClaimsPrincipal(
    new ClaimsIdentity(new List<Claim>
    {
        new Claim(ClaimTypes.Name, "Jon Doe"),
        new Claim(ClaimTypes.Role, Roles.UserRole)
    }));
```

And use him in our GraphQLUserContextBuilder:

```csharp
services
        .AddGraphQL(o =>
        {
            o.ExposeExceptions = true;
            o.ComplexityConfiguration = GraphQlConfig.ComplexityConfiguration;
            o.EnableMetrics = true;
        })
        .AddDataLoader()
        .AddGraphTypes()
        .AddUserContextBuilder(o => o.User = GraphQlConfig.FakedUser)
        .AddWebSockets();
```

Then we add it authentication and authorization in the pipeline:

```csharp
app.UseAuthentication()
    .UseAuthorization()
    .UseGraphQL<ISchema>();
```

Let's test out our faked user authorization, we should have an access to farms of persons:

```
# Query:
query test {
  persons { 
  	id, name, farms { id, name }
  }
}

# Results into:
{
  "data": {
    "persons": [
      {
        "id": "1",
        "name": "Mr. Jones",
        "farms": [
          {
            "id": "1",
            "name": "Manor Farm"
          }
        ]
      },
      {
        "id": "2",
        "name": "Mr. Whymper",
        "farms": [
          {
            "id": "2",
            "name": "AnimalFarm"
          }
        ]
      }
    ]
  }
}
```

But not an access to a secret piggy bank location of users:

```
# Query:
query test {
  persons { 
  	id, name, secretPiggyBankLocation
  }
}

# Results into:

{
  "errors": [
    {
      "message": "GraphQL.Validation.ValidationError: You are not authorized to run this query.\r\nRequired roles 'Admin' are not present.\r\n",
      "locations": [
        {
          "line": 5,
          "column": 5
        }
      ],
      "extensions": {
        "code": "authorization"
      }
    }
  ]
}
```

And if we make our faked user an admin, we have an access to the `SecretPiggyBankLocation` property:

```
{
  "data": {
    "persons": [
      {
        "id": "1",
        "name": "Mr. Jones",
        "secretPiggyBankLocation": "In a dark cave."
      },
      {
        "id": "2",
        "name": "Mr. Whymper",
        "secretPiggyBankLocation": "Does not have a piggy bank."
      }
    ]
  }
}
```

#### Complexity configuration

Executing a query based on client wishes can be dangerous and lead to easy DoS attacks. To prevent those, we can use `ComplexityConfiguration` of our GraphQL server:

```csharp
public static ComplexityConfiguration ComplexityConfiguration =>
    new ComplexityConfiguration
    {
        MaxDepth = 3,                   // Nested types in query
        FieldImpact = 3,                // average number of records in db
        MaxComplexity = 1_000           // max amount of returned fields in a query
    };
```

And add it to our GraphQL builder:

```csharp
services
      .AddGraphQL(o =>
      {
          o.ExposeExceptions = true;
          o.ComplexityConfiguration = GraphQlConfig.ComplexityConfiguration;
          o.EnableMetrics = true;
      })
```

Using this configuration, we can restrict the complexity of queries. It is recommended to set the `ComplexityConfiguration` to higher amount, the prediction of allowed max complexity can be tricky (https://graphql-dotnet.github.io/docs/getting-started/malicious-queries/).

###### Sources:

* https://code-maze.com/consume-graphql-api-with-asp-net-core/
* https://github.com/graphql-dotnet/graphql-client#usage
* https://stackoverflow.com/questions/53537521/how-to-implement-authorization-using-graphql-net-at-resolver-function-level/59885020#59885020
* https://graphql-dotnet.github.io/docs/getting-started/malicious-queries/
* https://www.learmoreseekmore.com/2020/01/dotnet-core-graphql-api-authorization.html
* https://fullstackmark.com/post/22/build-an-authenticated-graphql-app-with-angular-aspnet-core-and-identityserver-part-1
