# GraphQL ASP.NET Core demo (4 - Subscriptions)

### Subscriptions

Subscriptions provide a way to notify clients of real-time changes through a push model (usually WebSocked-based).

They are supported through the use of `IObservable<T>`. You will need a server that supports a Subscription protocol. The **GraphQL Server** project provides a .NET Core server that implements the *Apollo GraphQL* subscription protocol.


### Adding `IObservable<T>`
Let's suppose we want to know, when animal is created. To allow such subscription, we need to create an `IObservable<T>` collection of newly created Animals.
Extract `AnimalDataStore` from general `DataStore` and add the required `IObservable<Animal>`:


```csharp
private readonly Subject<Animal> animalCreated;
public IObservable<Animal> AnimalCreated => animalCreated.AsObservable();	
```

Then we have to invoke `OnNext()` method of our field `Subject<Entities.Animal> animalCreated`, which notifies all subscribed observers about a new animal in our create method `Task<Animal> CreateAnimalAsync(Entities.Animal animal)` after its creation:

```csharp
public async Task<Entities.Animal> CreateAnimalAsync(Entities.Animal animal)
{
	var addedAnimal = await dbContext.Animals.AddAsync(animal);
	await dbContext.SaveChangesAsync();

	animalCreated.OnNext(animal);	// <-- added this line of code
	return addedAnimal.Entity;
}
```

It is also important to have `AnimalDataStore` registered in our *IoC container* with **singleton lifetime**, let's register it in `ConfigureServices()` method in `Startup.cs`:

`services.AddSingleton<IAnimalDataStore, AnimalDataStore>();`

### Impementing Subscriptions
Having the `AnimalDataStore` prepared, we can create a class `AnimalSubscriptions` to provide the subscriptions GraphQL feature by our server. We will expose one subscription called *animalCreatedInFarm* which will fire whenever an animal is created to all observers subscribed to a specific farm.

Notable parts are `Resolver` and `Subscriber`. The does subscriptions class not have to be connected to an *Animal* entity, so we have to resolve the fired default object as an `Animal` type. `Subscriber` uses `EventStreamResolver` *(delegate for handling subscriptions on an event stream field)* and returns the newly created animals, to which the observer is subscribed:

```csharp
public class AnimalSubscriptions : ObjectGraphType
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
				new QueryArgument<IdGraphType>
				{
					Name = "farm"
				}),
				Type = typeof(AnimalCreatedEvent),
				Resolver = new FuncFieldResolver<Animal>(ResolveAnimal),
				Subscriber = new EventStreamResolver<Animal>(ctx => Subscribe(ctx, animalDataStore)),
			});
	}

	private Animal ResolveAnimal(ResolveFieldContext context)
	{
		return context.Source as Animal;
	}

	private IObservable<Animal> Subscribe(ResolveEventStreamContext context, IAnimalDataStore animalDataStore)
	{
		var farm = context.GetArgument<int>("farm");

		return animalDataStore
		.AnimalCreated
		.Where(x => farm == x.FarmId);
	}
}
```


### Enabling Subscriptions

We have to add created `AnimalSubscriptions` to our `AppSchema` as the Schema is the main part of GraphQL and dictates, what the server exposes:

```csharp
public class AppSchema : global::GraphQL.Types.Schema
{
	public AppSchema(IDependencyResolver resolver) : base(resolver)
	{
		Query = resolver.Resolve<AppQuery>();
		Mutation = resolver.Resolve<AnimalMutation>();
		Subscription = resolver.Resolve<AnimalSubscriptions>();
	}
}
```

As the subscriptions are realized through WebSockets, we have to register necessary classes using `IGraphQLBuilder` extension method `AddWebSockets()` in method `ConfigureServices()` of `Startup.cs`:

```csharp
services
	.AddGraphQL(o => { o.ExposeExceptions = true; })
	.AddDataLoader()
	.AddGraphTypes(ServiceLifetime.Scoped)
	.AddWebSockets();
```

And then use it in our middleware in `Configure()` method:

```csharp
// Use the GraphQL subscriptions in the specified schema and make them available.
app.UseWebSockets();
app.UseGraphQLWebSockets<ISchema>();
```



### GraphQL Subscriptions demo
> I decided to add `GraphQL.Playground` for easier testing of the subscriptions. The Playground will pop out on application startup.

Now we are able to subscribe to an animal creation events using the following query:

```
subscription AnimalAddedSubscription {
  animalCreatedInFarm(farm: 1) {
    id
    name
  }
}
```

This query will notify me, when an animal is created in a farm with ID = 1. To test notifications, we can try to create an animal in a subscribed and non-subscribed farm with the following query:

```
mutation AddAnimal($animal: AnimalInputType!) {
  addAnimal(newAnimal: $animal) {
    id
    name
    farmId
  }
}
```

And the query variables:

```
{
  "animal": {
    "name": "new animal second",
    "species": "unknown species",
    "farmId": 1
  }
}
```

We can see, that if we create animal with `"farmId": 1`, we get a notification:

```
{
  "data": {
    "animalCreatedInFarm": {
      "id": "67",
      "name": "new animal second"
    }
  }
}
```

And if we create an animal with farmId: 2, we get no notification.


###### Sources:

* https://graphql-dotnet.github.io/docs/getting-started/subscriptions
* https://elanderson.net/2018/08/graphql-using-net-boxed-subscriptions/
